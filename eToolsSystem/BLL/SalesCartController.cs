using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eTools.Data.DTOs;
using eToolsSystem.DAL;
#endregion

namespace eToolsSystem.BLL
{
    internal enum CartUpdateMode {INCREMENTAL, ABSOLUTE};

    [DataObject(true)]
    public class SalesCartController
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CartSummaryInfo ShoppingCart_GetSummary(int employeeID, string couponCode)
        {
            // get current cart, return null if cart not found
            using (var context = new eToolsContext())
            {
                ShoppingCart cart = context.ShoppingCarts.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
                return (cart == null) ? null :
                    new CartSummaryInfo
                    {
                        ShoppingCartID = cart.ShoppingCartID,
                        ProductCount = cart.ShoppingCartItems.Count(),
                        ItemCount = cart.ShoppingCartItems.Select(x => x.Quantity).Sum(),
                        Subtotal = cart.ShoppingCartItems.Select(x => x.Quantity * x.StockItem.SellingPrice).Sum(),
                        DiscountCoupon = Coupons_GetInfo(couponCode)
                    };
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<CartItemInfo> ShoppingCart_GetItems(int employeeID)
        {
            // get current cart, return null if cart not found
            using (var context = new eToolsContext())
            {
                ShoppingCart cart = context.ShoppingCarts.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
                return (cart == null) ? null :
                    cart.ShoppingCartItems.Select(i => new CartItemInfo
                        {
                            CartItemID = i.ShoppingCartItemID,
                            ProductID = i.StockItem.StockItemID,
                            ProductDescription = i.StockItem.Description,
                            SellingPrice = i.StockItem.SellingPrice,
                            QtyOnHand = i.StockItem.QuantityOnHand,
                            QtyInCart = i.Quantity
                        }).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public CouponInfo Coupons_GetInfo(string couponCode)
        {
            using (var context = new eToolsContext())
            {
                return context.Coupons
                    .Where(c => c.CouponIDValue == couponCode)
                    .Select(c => new CouponInfo
                        {
                            CouponID = c.CouponID,
                            CouponCode = c.CouponIDValue,
                            StartDate = c.StartDate,
                            EndDate = c.EndDate,
                            DiscountPercent = c.CouponDiscount
                        })
                    .FirstOrDefault();
            }
        }

        public SalesReceipt PlaceOrder(int employeeId, string couponCode, string paymentMethod)
        {
            using (var context = new eToolsContext())
            {
                Employee employee = context.Employees.Find(employeeId);
                CartSummaryInfo cartSummary = ShoppingCart_GetSummary(employeeId, couponCode);
                List<CartItemInfo> cartItems = ShoppingCart_GetItems(employeeId);

                Dictionary<string, string> validPaymentMethods = new Dictionary<string, string>
                {
                    {"M", "Money" },
                    {"C", "Credit" },
                    {"D", "Debit" }
                };

                List<string> errors = new List<string>();
                if (employee == null)
                {
                    errors.Add("Employee not found.");
                }
                else if (cartSummary == null)
                {
                    errors.Add("Employee has no shopping cart.");
                }
                else if (cartItems == null || cartItems.Count == 0)
                {
                    errors.Add("Shopping cart is empty.");
                }
                else if (!string.IsNullOrWhiteSpace(couponCode) && cartSummary.DiscountCoupon == null)
                {
                    errors.Add("Coupon code invalid.");
                }
                else if (cartSummary.DiscountCoupon != null && !cartSummary.DiscountCoupon.IsValid)
                {
                    errors.Add("Coupon expired.");
                }
                if (string.IsNullOrWhiteSpace(paymentMethod))
                {
                    errors.Add("Payment method must not be empty.");
                }
                else if (!validPaymentMethods.ContainsKey(paymentMethod))
                {
                    errors.Add("Payment method invalid.");
                }

                // validate each item in the cart
                foreach (var item in cartItems)
                {
                    StockItem product = context.StockItems.Find(item.ProductID);
                    List<string> itemErrors = ValidateCartItem(employee, product, item.QtyInCart);
                    errors.AddRange(itemErrors);
                }

                if (errors.Count > 0)
                {
                    throw new BusinessRuleException("Order cannot be placed.", errors);
                }
                else
                {
                    // all validation passed, build sales receipt
                    var receipt = new SalesReceipt()
                    {
                        SaleDate = DateTime.Now,
                        PaymentType = paymentMethod,
                        PaymentTypeDesc = validPaymentMethods[paymentMethod],
                        EmployeeId = employeeId,
                        EmployeeName = employee.LastName + ", " + employee.FirstName,
                        SubTotal = cartSummary.Subtotal,
                        CouponId = cartSummary.DiscountCoupon?.CouponID,
                        CouponCode = couponCode,
                        DiscountPercent = (cartSummary.DiscountCoupon == null) ? 0 : cartSummary.DiscountCoupon.DiscountPercent,
                        DiscountAmount = cartSummary.Discount,
                        TaxAmount = cartSummary.GST,
                        TotalAmount = cartSummary.Total,
                        Items = cartItems.Select(item => new SalesReceiptItem
                        {
                            ProductId = item.ProductID,
                            ProductName = item.ProductDescription,
                            SellingPrice = item.SellingPrice,
                            Qty = item.QtyInCart,
                            SubTotal = item.TotalPrice
                        }).ToList()
                    };

                    // store new sale
                    Sale newSale = context.Sales.Add(new Sale()
                    {
                        SaleDate = receipt.SaleDate,
                        PaymentType = receipt.PaymentType,
                        EmployeeID = receipt.EmployeeId,
                        TaxAmount = receipt.TaxAmount,
                        SubTotal = receipt.SubTotal,
                        CouponID = receipt.CouponId
                    });
                    if (newSale == null)
                    {
                        throw new Exception("Sale cannot be added to the database.");
                    }
                    // process each item: insert into the database and decrease qty on hand
                    foreach (var item in receipt.Items)
                    {
                        SaleDetail newSaleDetail = context.SaleDetails.Add(new SaleDetail()
                        {
                            SaleID = newSale.SaleID,
                            StockItemID = item.ProductId,
                            SellingPrice = item.SellingPrice,
                            Quantity = item.Qty,
                            Backordered = false,
                            ShippedDate = null
                        });
                        if (newSaleDetail == null)
                        {
                            throw new Exception("Sale detail cannot be added to the database.");
                        }
                        // decrease stock
                        StockItem currentProduct = context.StockItems.Find(item.ProductId);
                        currentProduct.QuantityOnHand -= item.Qty;
                        context.Entry(currentProduct).Property(x => x.QuantityOnHand).IsModified = true;
                        if (currentProduct.QuantityOnHand < 0)
                        {
                            throw new Exception("Product out of stock.");
                        }
                    }
                    // all good, delete cart
                    context.ShoppingCartItems.RemoveRange(
                        context.ShoppingCartItems.Where(x => x.ShoppingCart.EmployeeID == employeeId));
                    // delete shopping cart (I used RemoveRange just to make it simpler)
                    context.ShoppingCarts.RemoveRange(
                        context.ShoppingCarts.Where(x => x.EmployeeID == employeeId));

                    // commit all changes
                    context.SaveChanges();

                    // retrieve receipt ID
                    receipt.SaleID = newSale.SaleID;

                    return receipt;
                }
            }
        }

        public void CancelCart(int employeeID)
        {
            using (var context = new eToolsContext())
            {
                // delete shopping cart items for employeeID
                context.ShoppingCartItems.RemoveRange(
                    context.ShoppingCartItems.Where(x => x.ShoppingCart.EmployeeID == employeeID));
                // delete shopping cart (I used RemoveRange just to make it simpler)
                context.ShoppingCarts.RemoveRange(
                    context.ShoppingCarts.Where(x => x.EmployeeID == employeeID));
                // commit changes
                context.SaveChanges();
            }
        }
            // This was the assignment requirement. See the implemented version below.
            public void AddToCart(string username, int productId, int quantity)
        {
            // check the username, must be registered and must have Sales role
            throw new NotImplementedException("eToolsSystem cannot access AppSecurity, so this method cannot be implemented.");

        }
        // Adds the product with the given quantity into the cart under the given user and decreases the stock on hand for the given product.
        // If the product ID is invalid, or the product has not enough stock on hand, it raises an exception.
        //Actions:
        //-	If the user has no cart yet, it creates the cart first, otherwise updates the last update timestamp of the cart
        //        -	If the product already exists in the cart, increases the qty in the cart.
        //-	If the product is not yet in the cart, inserts the product into the cart.

        public void DeleteCartItem(int employeeId, int productId)
        {
            using (var context = new eToolsContext())
            {
                // collect entities
                Employee employee = context.Employees.Find(employeeId);
                StockItem product = context.StockItems.Find(productId);
                ShoppingCart cart = context.ShoppingCarts.Where(x => x.EmployeeID == employeeId).FirstOrDefault();
                ShoppingCartItem item = cart?.ShoppingCartItems.Where(x => x.StockItemID == productId).FirstOrDefault();

                List<string> errors = new List<string>();
                if (employee == null)
                {
                    errors.Add("Employee not found.");
                }
                else if (cart == null)
                {
                    errors.Add("Employee has no shopping cart.");
                }
                if (product == null)
                {
                    errors.Add("Product not found.");
                }
                else if (item == null)
                {
                    errors.Add("Product not in cart.");
                }
                if (errors.Count > 0)
                {
                    throw new BusinessRuleException("Product cannot be removed from cart.", errors);
                }
                else
                {
                    var existingItem = context.ShoppingCartItems.Find(item.ShoppingCartItemID);
                    if (existingItem == null)
                    {
                        throw new Exception("Product does not exists in shopping cart on file.");
                    }
                    context.ShoppingCartItems.Remove(existingItem);
                    // check if this was the last product in the cart, if yes, delete the cart
                    var existingCart = context.ShoppingCarts.Find(cart.ShoppingCartID);
                    if (existingCart == null)
                    {
                        throw new Exception("Shopping cart does not exist on file.");
                    }
                    if (existingCart.ShoppingCartItems.Count == 0)
                    {
                        context.ShoppingCarts.Remove(existingCart);
                    }
                    context.SaveChanges();
                }
            }
        }
        public void UpdateCart(int employeeId, int productId, int newQuantity)
        {
            CreateOrUpdateCart(employeeId, productId, newQuantity, CartUpdateMode.ABSOLUTE);
        }
        public void AddToCart(int employeeId, int productId, int quantityToAdd)
        {
            CreateOrUpdateCart(employeeId, productId, quantityToAdd, CartUpdateMode.INCREMENTAL);
        }
        private void CreateOrUpdateCart(int employeeId, int productId, int quantity, CartUpdateMode mode)
        {
            using (var context = new eToolsContext())
            {
                // collect entities
                Employee employee = context.Employees.Find(employeeId);
                StockItem product = context.StockItems.Find(productId);

                // validate request
                List<string> errors = ValidateCartItem(employee, product, quantity);
                if (errors.Count > 0)
                {
                    string errorMessage = (mode == CartUpdateMode.INCREMENTAL) ? 
                        "Product not added to cart." : 
                        "Product not updated in cart.";
                    throw new BusinessRuleException(errorMessage, errors);
                }
                else
                {
                    // validation passed, update cart
                    CreateOrUpdateCartValidated(employee, product, quantity, mode);
                }
            }
        }

        // Update cart and cartitems. Must be called with validated parameters.
        private void CreateOrUpdateCartValidated(Employee employee, StockItem product, int quantity, CartUpdateMode mode)
        {
            // handle cart update in one transaction
            using (var context = new eToolsContext())
            {
                // find shopping cart
                ShoppingCart cart = context.ShoppingCarts.Where(x => x.EmployeeID == employee.EmployeeID).FirstOrDefault();
                // create new cart if not found or update existing cart
                if (cart == null)
                {
                    // create new cart
                    cart = new ShoppingCart();
                    cart.EmployeeID = employee.EmployeeID;
                    cart.CreatedOn = DateTime.Now;
                    context.ShoppingCarts.Add(cart);
                }
                else
                {
                    // update existing cart
                    cart.UpdatedOn = DateTime.Now;
                    context.Entry(cart).Property(x => x.UpdatedOn).IsModified = true;
                    // context.Entry(cart).State = System.Data.Entity.EntityState.Modified;
                }

                // find cart item in cart
                ShoppingCartItem item = cart.ShoppingCartItems.Where(x => x.StockItemID == product.StockItemID).FirstOrDefault();
                // add item to cart if not found or update qty if found
                if (item == null)
                {
                    // add item to cart
                    item = new ShoppingCartItem();
                    item.ShoppingCartID = cart.ShoppingCartID;
                    item.StockItemID = product.StockItemID;
                    item.Quantity = quantity;
                    context.ShoppingCartItems.Add(item);
                }
                else
                {
                    // update existing item in cart
                    if (mode == CartUpdateMode.INCREMENTAL)
                    {
                        item.Quantity += quantity;
                        if (item.Quantity > product.QuantityOnHand)
                        {
                            throw new BusinessRuleException(
                                "Product not added to cart.",
                                string.Format("Not enough stock: requested {0} available {1}.",
                                    item.Quantity, 
                                    product.QuantityOnHand));
                        }
                    }
                    else
                    {
                        item.Quantity = quantity;
                        if (item.Quantity > product.QuantityOnHand)
                        {
                            throw new BusinessRuleException(
                                "Product not added to cart.",
                                string.Format("Not enough stock: requested {0} available {1}.",
                                    item.Quantity,
                                    product.QuantityOnHand));
                        }
                    }
                    context.Entry(item).Property(x => x.Quantity).IsModified = true;
                }

                // commit transaction
                context.SaveChanges();
            }

        }
        List<string> ValidateCartItem(Employee employee, StockItem product, int quantity)
        {
            List<string> errors = new List<string>();
            if (employee == null)
            {
                errors.Add("Employee not found.");
            }
            if (product == null)
            {
                errors.Add("Product not found.");
            }
            else if (product.Discontinued)
            {
                errors.Add("Product is discontinued, cannot be added to cart.");
            }
            else if (product.QuantityOnHand <= 0)
            {
                errors.Add("Product is out of stock, cannot be added to cart.");
            }
            else
            {
                if (quantity < 1)
                {
                    errors.Add("Quantity to add must be at least 1.");
                }
                else if (product.QuantityOnHand < quantity)
                {
                    errors.Add(string.Format("Not enough stock: requested {0} available {1}.", quantity, product.QuantityOnHand));
                }
            }
            return errors;
        }
    }
}
