using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
using AppSecurity.BLL;
using eTools.Data.POCOs;
using eTools.Data.DTOs;
using eToolsSystem.BLL;
using eToolsWebsite.Helpers;
using Microsoft.AspNet.Identity.Owin;
#endregion

namespace eToolsWebsite.WebPages
{
    public partial class Sales : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // refresh cart summary at initial page load
            if (!IsPostBack)
            {
                ShowReceipt(null);
                RefreshCartSummary();
            }
        }

        protected void RefreshCartSummary()
        {
            // check if the user is logged in, has sales role and is an employee and set visibility accordingly
            if (CurrentEmployee == null)
            {
                // not logged in or not employee or has no sales role
                CartSummaryPanelNotLoggedIn.Visible = true;
                CartSummaryPanel.Visible = false;
                // disable checkout button in navigation
                setActiveTab(false);
            }
            else
            {
                // logged in and employee with sales role
                CartSummaryPanelNotLoggedIn.Visible = false;
                CartSummaryPanel.Visible = true;

                // set salesperson name
                CartSummary_EmployeeName.Text = CurrentEmployeeName;
                CartSummary_EmployeeID.Text = CurrentEmployeeID.ToString();

                // get cart summary for current employee given the coupon code
                var cartmgr = new SalesCartController();
                CartSummaryInfo summary = cartmgr.ShoppingCart_GetSummary(CurrentEmployeeID, CartSummary_Coupon.Text);

                // if the cart is empty, display a message, otherwise display the cart summary
                if (summary == null)
                {
                    // cart is empty
                    CartSummaryPanelEmpty.Visible = true;
                    CartSummaryPanelNotEmpty.Visible = false;
                    // disable checkout button in navigation
                    setActiveTab(false);
                }
                else
                {
                    // cart is not empty, populate fields
                    CartSummaryPanelEmpty.Visible = false;
                    CartSummaryPanelNotEmpty.Visible = true;
                    // enable checkout button in navigation
                    setActiveTab(true);
                    // populate cart summary fields
                    CartSummary_ProductCount.Text = string.Format("{0}", summary.ProductCount);
                    CartSummary_ItemCount.Text = string.Format("{0}", summary.ItemCount);
                    CartSummary_SubtotalAmount.Text = string.Format("{0:C2}", summary.Subtotal);
                    CartSummary_DiscountAmount.Text = string.Format("{0:C2}", summary.Discount);
                    CartSummary_TaxAmount.Text = string.Format("{0:C2}", summary.GST);
                    CartSummary_TotalAmount.Text = string.Format("{0:C2}", summary.Total);

                    // check coupon and display discount percent if coupon was valid or error otherwise
                    if (string.IsNullOrWhiteSpace(CartSummary_Coupon.Text))
                    {
                        // coupon field empty, hide all messages
                        CartSummary_DicountIcon_OK.Visible = false;
                        CartSummary_DiscontIcon_Error.Visible = false;
                        CartSummary_DiscountError.Text = "";
                        CartSummary_DiscountPercent.Text = "";
                    }
                    else if (summary.DiscountCoupon == null)
                    {
                        // coupon not found
                        CartSummary_DicountIcon_OK.Visible = false;
                        CartSummary_DiscontIcon_Error.Visible = true;
                        CartSummary_DiscountError.Text = "Invalid";
                        CartSummary_DiscountPercent.Text = "";
                    }
                    else if (!summary.DiscountCoupon.IsValid)
                    {
                        // coupon not valid because of date range
                        CartSummary_DicountIcon_OK.Visible = false;
                        CartSummary_DiscontIcon_Error.Visible = true;
                        CartSummary_DiscountError.Text = "Expired";
                        CartSummary_DiscountPercent.Text = "";
                    }
                    else
                    {
                        // coupon valid, dispay percentage
                        CartSummary_DicountIcon_OK.Visible = true;
                        CartSummary_DiscontIcon_Error.Visible = false;
                        CartSummary_DiscountError.Text = "";
                        CartSummary_DiscountPercent.Text = string.Format("{0} %", summary.DiscountCoupon.DiscountPercent);
                    }

                }
            }
            // refresh cart items
            CartItemsView.DataBind();
        }
        protected bool IsSalesUser
        {
            get
            {
                return Request.IsAuthenticated && User.IsInRole(SecurityRoles.Sales);
            }
        }

        // add selected item to cart
        protected void AddToCartButton_Click(object sender, EventArgs e)
        {
            // find the value of the qty to add textbox
            // First get the grandparent from control hierarchy: ListViewDataItem --> TableCell --> Sender button
            // then get the values from its controls
            Control item = ControlHelper.GetParent(sender, 2);
            string productName = ControlHelper.GetControlValue(item, "ProductDescriptionLabel");
            int productID = ControlHelper.GetControlValue(item, "ProductIDLabel", -1);
            int qtyOnHand = ControlHelper.GetControlValue(item, "QtyOnHandLabel", -1);
            int qtyToAdd  = ControlHelper.GetControlValue(item, "AddToCartQty",   -1);

            // generate our messages
            string unit = (qtyToAdd > 1) ? "units" : "unit";
            string successMessage = 
                string.Format("<strong>{0}</strong> {1} of <strong>{2}</strong> added to cart.", 
                qtyToAdd, unit, productName);
            string failureMessage = "Product not added to cart.";


            MessageUserControl.TryRun(() =>
            {
                // check if user is logged in
                if (!IsSalesUser || CurrentEmployee == null)
                {
                    throw new BusinessRuleException(failureMessage, "User must be employee, logged in and have Sales role to add products to cart.");
                }
                // check product id
                else if (productID <= 0)
                {
                    throw new BusinessRuleException(failureMessage, "Product ID must be positive integer.");
                }
                // check out of stock
                else if (qtyOnHand <= 0)
                {
                    throw new BusinessRuleException(failureMessage, "Product is out of stock, cannot be added to cart.");
                }
                // check qty to add
                else if (qtyToAdd <= 0 || qtyToAdd > qtyOnHand)
                {
                    throw new BusinessRuleException(failureMessage, "Quantity to add must be between 1 and quantity on stock.");
                }
                // yay, we can try and add the product to the cart
                else
                {
                    // All validation passed, we can safely call our BLL. 
                    // Now, our BLL cannot access the AppSecurity assembly, so we needed to get the Employee ID and 
                    // pass that to our BLL instead of the username:
                    SalesCartController cartmgr = new SalesCartController();
                    cartmgr.AddToCart(CurrentEmployeeID, productID, qtyToAdd);
                    // refresh cart summary
                    // CartSummaryView.DataBind();
                    RefreshCartSummary();
                }
            }, "Success", successMessage);
        }

        #region TODO: Remove before flight
        // TODO: Delete this method before shipping the software
        protected void TestLogin_Click(object sender, EventArgs e)
        {

            // login credentials for a demo sales user
            const string demoUser = "mPants";
            const string demoPassword = "Pa$$word1";
            const bool demnoRemember = true;

            MessageUserControl.TryRun(() =>
                {
                    // log in with a demo sales user
                    var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var signinManager = Context.GetOwinContext().GetUserManager<ApplicationSignInManager>();
                    var result = signinManager.PasswordSignIn(demoUser, demoPassword, demnoRemember, shouldLockout: false);

                    if (result != SignInStatus.Success)
                    {
                        throw new BusinessRuleException("Sign in failed", string.Format("User {0} could not log in.", demoUser));
                    }
                }, "Success", string.Format("User {0} logged in.", demoUser));
            IdentityHelper.RedirectToReturnUrl("/WebPages/Sales/Sales", Response);
        }
        #endregion

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                if (!IsSalesUser || CurrentEmployee == null)
                {
                    throw new BusinessRuleException("Cannot delete cart", "User must be employee, logged in and have Sales role to add products to cart.");
                }
                else
                {
                    // delete cart and cart items
                    SalesCartController cartmgr = new SalesCartController();
                    cartmgr.CancelCart(CurrentEmployeeID);

                    // clear previous coupon code
                    CartSummary_Coupon.Text = "";

                    // refresh cart
                    RefreshCartSummary();
                }
                SalesTabView.SetActiveView(TabShopping);
            }, "Cart deleted", "All items deleted from the current shopping cart.");
        }

        protected string CurrentEmployeeName
        {
            get
            {
                string employeeName = (CurrentEmployee != null) ? CurrentEmployee.FullName : null;
                return employeeName;
            }
        }
        protected int CurrentEmployeeID
        {
            get
            {
                int employeeID = (CurrentEmployee != null) ? CurrentEmployee.EmployeeID : -1;
                return employeeID;
            }
        }
        protected EmployeeInfo CurrentEmployee
        {
            get
            {
                // get employee info from current user or null if not logged in or not sales user.
                // employee ID is -1 if not logged in or has no sales role or not an employee
                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                EmployeeInfo currentEmployee = (IsSalesUser) ? manager.User_GetEmployee(User.Identity.Name) : null;
                return currentEmployee;
            }
        }

        protected void DataBindOnLoad(object sender, EventArgs e)
        {
            if ((sender as Control) != null)
            {
                (sender as Control).DataBind();
            }
        }

        protected void CartSummary_Coupon_TextChanged(object sender, EventArgs e)
        {
            RefreshCartSummary();
        }

        private void setActiveTab(bool enableCheckout)
        {
            setActiveTab(SalesTabView.ActiveViewIndex, enableCheckout);
        }
        private void setActiveTab(int tabindex)
        {
            setActiveTab(tabindex, btnTabCheckout.Enabled);
        }
        private void setActiveTab(int tabindex, bool enableCheckout)
        {
            // force shopping tab (index 0) if checkout is disabled
            SalesTabView.ActiveViewIndex = (enableCheckout) ? tabindex : 0;
            btnTabCheckout.Enabled = enableCheckout;
            btnTabShopping.DataBind();
            btnTabCheckout.DataBind();
        }
        protected void btnTabShopping_Click(object sender, EventArgs e)
        {
            RefreshCartSummary();
            setActiveTab(0);
        }

        protected void btnTabCheckout_Click(object sender, EventArgs e)
        {
            setActiveTab(1);
        }
        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {

            string couponCode = CartSummary_Coupon?.Text;
            string paymentMethod = PaymentTypeDDL?.SelectedValue;

            MessageUserControl.TryRun( () => {
                SalesCartController cartmgr = new SalesCartController();
                SalesReceipt receipt = cartmgr.PlaceOrder(CurrentEmployeeID, couponCode, paymentMethod);
                // clear the coupon code and payment method for the next customer
                CartSummary_Coupon.Text = "";
                PaymentTypeDDL.SelectedIndex = 0;
                // switch the display to sales receipt mode and display receipt
                ShowReceipt(receipt);
            }, "Success", "Order registered. Use the top menu bar to start a new sale.");

        }

        private void ShowReceipt(SalesReceipt receipt)
        {
            if (receipt == null)
            {
                // switch screen to shopping mode
                btnTabShopping.Visible = true;
                btnTabCheckout.Visible = true;
                CartSummary.Visible = true;
                SalesTabView.SetActiveView(TabShopping);
                RefreshCartSummary();
            }
            else
            {
                // switch screen to receipt mode and display receipt
                btnTabShopping.Visible = false;
                btnTabCheckout.Visible = false;
                CartSummary.Visible = false;
                SalesTabView.SetActiveView(TabReceipt);

                // header
                Receipt_ID.Text = receipt.SaleID.ToString();
                Receipt_Date.Text = string.Format("{0:F}", receipt.SaleDate);
                Receipt_EmployeeName.Text = receipt.EmployeeName;
                Receipt_PaymentType.Text = receipt.PaymentTypeDesc;

                // footer
                Receipt_Subtotal.Text = string.Format("{0:C2}", receipt.SubTotal);
                Receipt_DiscountLine.Visible = (receipt.DiscountPercent > 0);
                Receipt_DiscountCoupon.Text = "Discount for coupon " + receipt.CouponCode;
                Receipt_DiscountPercent.Text = string.Format("{0}%", receipt.DiscountPercent);
                Receipt_DiscountAmount.Text = string.Format("{0:C2}", -receipt.DiscountAmount);
                Receipt_TaxAmount.Text = string.Format("{0:C2}", receipt.TaxAmount);
                Receipt_TotalAmount.Text = string.Format("{0:C2}", receipt.TotalAmount);

                // items
                ReceiptItemList.DataSource = receipt.Items;
                ReceiptItemList.DataBind();
            }
        }

        protected void btnDeleteCartItem_Click(object sender, EventArgs e)
        {
            // delete selected product from cart
            Control item = ControlHelper.GetParent(sender, 2);
            string productName = ControlHelper.GetControlValue(item, "ProductDescriptionLabel");
            int productID = ControlHelper.GetControlValue(item, "ProductIDLabel", -1);

            // generate our messages
            string successMessage =
                string.Format("<strong>{0}</strong> removed from cart.", productName);
            string failureMessage =
                string.Format("Cannot remove propduct <strong>{0}</strong> (ID={1}) from cart.", productName, productID);

            MessageUserControl.TryRun(() =>
            {
                // check if user is logged in
                if (!IsSalesUser || CurrentEmployee == null)
                {
                    throw new BusinessRuleException(failureMessage, "User must be employee, logged in and have Sales role to add products to cart.");
                }
                // check product id
                else if (productID <= 0)
                {
                    throw new BusinessRuleException(failureMessage, "Product ID must be positive integer.");
                }
                // yay, we can try and remove the product from the cart
                else
                {
                    // All validation passed, we can safely call our BLL. 
                    // Now, our BLL cannot access the AppSecurity assembly, so we needed to get the Employee ID and 
                    // pass that to our BLL instead of the username:
                    SalesCartController cartmgr = new SalesCartController();
                    cartmgr.DeleteCartItem(CurrentEmployeeID, productID);
                    // refresh cart summary
                    // CartSummaryView.DataBind();
                    RefreshCartSummary();
                }
            }, "Success", successMessage);

        }
        protected void btnDecCartItem_Click(object sender, EventArgs e)
        {
            // decrease selected product qty in cart
            updateCartItem(sender, -1);

        }
        protected void btnIncCartItem_Click(object sender, EventArgs e)
        {
            // increase selected product qty in cart
            updateCartItem(sender, 1);
        }

        protected void btnUpdateCartItem_Click(object sender, EventArgs e)
        {
            updateCartItem(sender, 0);
        }

        private void updateCartItem(object sender, int delta)
        {
            // find the value of the qty to add textbox
            // First get the grandparent from control hierarchy: ListViewDataItem --> TableCell --> Sender button
            // then get the values from its controls
            Control item = ControlHelper.GetParent(sender, 2);
            string productName = ControlHelper.GetControlValue(item, "ProductDescriptionLabel");
            int productID = ControlHelper.GetControlValue(item, "ProductIDLabel", -1);
            int qtyOnHand = ControlHelper.GetControlValue(item, "QtyOnHandLabel", -1);
            int qtyInCart = ControlHelper.GetControlValue(item, "QtyInCart", -1);
            int newQty = qtyInCart + delta;

            // generate our messages
            string unit = (newQty > 1) ? "units" : "unit";
            string successMessage =
                string.Format("Cart updated to <strong>{0}</strong> {1} of <strong>{2}</strong>.",
                newQty, unit, productName);
            string failureMessage = "Cart cannot be updated for " + productName + " id=" + productID + " qty="+qtyInCart + " new="+newQty+" stock="+qtyOnHand;

            MessageUserControl.TryRun(() =>
            {
                // check if user is logged in
                if (!IsSalesUser || CurrentEmployee == null)
                {
                    throw new BusinessRuleException(failureMessage, "User must be employee, logged in and have Sales role to add products to cart.");
                }
                // check product id
                else if (productID <= 0)
                {
                    throw new BusinessRuleException(failureMessage, "Product ID must be positive integer.");
                }
                // check out of stock
                else if (qtyOnHand <= 0 && newQty > 0)
                {
                    throw new BusinessRuleException(failureMessage, "Product is out of stock, must be deleted from cart.");
                }
                // check qty to add
                else if (newQty < 1 || newQty > qtyOnHand)
                {
                    throw new BusinessRuleException(failureMessage, string.Format("Quantity must be between 1 and quantity on stock {0}.", qtyOnHand));
                }
                // yay, we can try and add the product to the cart
                else
                {
                    // All validation passed, we can safely call our BLL. 
                    // Now, our BLL cannot access the AppSecurity assembly, so we needed to get the Employee ID and 
                    // pass that to our BLL instead of the username:
                    SalesCartController cartmgr = new SalesCartController();
                    cartmgr.UpdateCart(CurrentEmployeeID, productID, newQty);
                    // refresh cart summary
                    // CartSummaryView.DataBind();
                    RefreshCartSummary();
                }
            }, "Success", successMessage);

        }
    }

}
