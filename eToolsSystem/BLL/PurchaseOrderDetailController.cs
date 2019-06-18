using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region More Namespaces
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class PurchaseOrderDetailController
    {
        /// <summary>
        /// Retrieves all details for an outstanding order (with a specified ID)
        /// </summary>
        /// <param name="_purchaseOrderID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OpenOrderDetail> OpenOrderDetail_List(int _purchaseOrderID)
        {
            using (var context = new eToolsContext())
            {
                var OpenOrderDetails = (
                    from x in context.PurchaseOrderDetails
                    where x.PurchaseOrderID == _purchaseOrderID
                    select new OpenOrderDetail
                    {
                        POID = x.PurchaseOrderID,
                        PODetailID = x.PurchaseOrderDetailID,
                        QtyOrdered = x.Quantity,
                        StockItemID = x.StockItemID,
                        Description = x.StockItem.Description,

                        // Outstanding = (ordered qty - received qty)
                        QtyOutstanding = (x.ReceiveOrderDetails.Count() == 0) // In the event stock item has never been received before
                            ? x.Quantity // Set outstanding qty to ordered qty
                            : x.Quantity - (
                            from y in x.ReceiveOrderDetails
                            select y.QuantityReceived)
                            .Sum()
                    })
                    .ToList();

                return OpenOrderDetails;
            }
        }
        public List<CurrentActiveOrderList> List_StockItemsForSuggestedNewOrder( int vendorid,int employeeid  )           
        {
            using (var context = new eToolsContext())
            {
                //check if vendor already has current open order
                 var exists = context.PurchaseOrders.
                    Where(x => x.VendorID.Equals(vendorid) && x.OrderDate==null).Select(x => x).FirstOrDefault();

                   
                 if (exists != null)
                    {
                    var results = (from x in context.PurchaseOrderDetails
                                   where x.PurchaseOrder.VendorID == exists.VendorID &&
                                  x.PurchaseOrder.PurchaseOrderNumber == null && x.PurchaseOrder.OrderDate == null
                                   orderby x.StockItem.Description
                                   select new CurrentActiveOrderList
                                   {
                                      
                                       StockItemID = x.StockItem.StockItemID,
                                       Description = x.StockItem.Description,
                                       QuantityOnHand = x.StockItem.QuantityOnHand,
                                       QuantityOnOrder = x.StockItem.QuantityOnOrder,
                                       ReOrderLevel = x.StockItem.ReOrderLevel,
                                       QuantityToOrder =  x.Quantity,
                                       Price = Math.Round(x.StockItem.PurchasePrice,2)
                                   });
                   
                    return results.ToList();
                }
                else
                {
                    var results = (from x in context.StockItems
                                   where x.VendorID == vendorid &&
                                 x.ReOrderLevel > (x.QuantityOnHand + x.QuantityOnOrder)
                                   orderby x.Description
                                   select new CurrentActiveOrderList
                                   {
                                     
                                       StockItemID = x.StockItemID,
                                       Description = x.Description,
                                       QuantityOnHand = x.QuantityOnHand,
                                       QuantityOnOrder = x.QuantityOnOrder,
                                       ReOrderLevel = x.ReOrderLevel,
                                       QuantityToOrder =1,
                                       Price = Math.Round( x.PurchasePrice,2)
                                   });

                   // I must create a new purchase order record
                    List<string> reasons = new List<string>();
                  
                        decimal sub = 0;                     
                    PurchaseOrder info = info = new PurchaseOrder();
                    info.EmployeeID = employeeid;
                    info.VendorID = vendorid;
                    info = context.PurchaseOrders.Add(info);            

                   
                        PurchaseOrderDetail neworderdetails = null;

                       foreach(var temp in results.ToList())
                        {
                            neworderdetails = new PurchaseOrderDetail();
                            neworderdetails.StockItemID = temp.StockItemID;
                            neworderdetails.PurchasePrice = temp.Price;
                            neworderdetails.Quantity = temp.QuantityToOrder;
                            sub+= (temp.Price) * (decimal)(temp.QuantityToOrder);                          
                            info.PurchaseOrderDetails.Add(neworderdetails);               
                        } 
                                              
                        info.SubTotal = Math.Round(sub,2);
                        info.TaxAmount= Math.Round((sub * (decimal)0.05), 2);
                        //total = Math.Round(sub, 2) + Math.Round((sub * (decimal)0.05), 2);
                    //}
                    context.SaveChanges();
                    return results.ToList();
                }
            }
        }
        public void Add_LinItemToCurrentActivePurchaseOrdderList( int vendorid, int stockitemid)
        {
            List<string> reasons = new List<string>();
            using (var context = new eToolsContext())
            {
                var order = context.PurchaseOrders
                  .Where(or => or.VendorID == vendorid && or.OrderDate == null)
                  .Select(or => or).FirstOrDefault();
               
                PurchaseOrderDetail orderdetails = null;                             
                
                    orderdetails = new PurchaseOrderDetail();
                decimal sub1 = 0;
                    orderdetails.StockItemID = stockitemid;
                    orderdetails.Quantity = 1;
                    orderdetails.PurchasePrice = (from c in context.StockItems
                                                 where c.VendorID==vendorid &&
                                                 c.StockItemID==stockitemid
                                                 select c.PurchasePrice).FirstOrDefault();
                     orderdetails.PurchaseOrderID = order.PurchaseOrderID;

                    sub1 = orderdetails.PurchasePrice * orderdetails.Quantity;

                order.SubTotal = order.SubTotal + sub1;
                order.TaxAmount = order.TaxAmount + (sub1 * (decimal)0.05);
                context.PurchaseOrderDetails.Add(orderdetails);
                context.SaveChanges();
            }                   
            }  

        public void Remove_CurrentActivePurchaseOrdder( int vendorid, int stockitmeid)
        {
            List<string> reasons = new List<string>();
            using (var context = new eToolsContext())
            {
                var exist = context.PurchaseOrders
                    .Where(c => c.VendorID == vendorid && c.OrderDate==null).Select(c => c).FirstOrDefault();
                if(exist==null)

                {
                    throw new Exception("Purchase Order has been removed from the files.");
                }
                else
                {
                    PurchaseOrderDetail item = null;
                    item = exist.PurchaseOrderDetails.
                        Where(ord => ord.StockItemID==stockitmeid).FirstOrDefault();
                  if(item!=null)
                    {
                        exist.SubTotal = Math.Round(exist.SubTotal - (item.Quantity * (decimal)(item.PurchasePrice)), 2);
                        exist.TaxAmount = Math.Round((exist.SubTotal * (decimal)0.05), 2);
                        context.PurchaseOrderDetails.Remove(item);                   
                    }            

                }
                context.SaveChanges();
               }
            }
        public List<CurrentActiveOrderList> list_AddedCurerentActiveOrder(int vendorid)
        {
            using (var context = new eToolsContext())
            {

                var theorder = (from x in context.PurchaseOrderDetails
                                where x.PurchaseOrder.VendorID == vendorid
                                && x.PurchaseOrder.OrderDate == null
                                orderby x.StockItem.Description
                                select new CurrentActiveOrderList
                                {
                                    StockItemID = x.StockItem.StockItemID,
                                    Description = x.StockItem.Description,
                                    QuantityOnHand = x.StockItem.QuantityOnHand,
                                    QuantityOnOrder = x.StockItem.QuantityOnOrder,
                                    ReOrderLevel = x.StockItem.ReOrderLevel,
                                    QuantityToOrder = x.Quantity,
                                    Price = Math.Round( x.PurchasePrice,2)
                                });
                return theorder.ToList();
            }
        }
        public List<CurrentActiveOrderList> list_PlacedCurerentActiveOrder(int vendorid)
        {
            using (var context = new eToolsContext())
            {
               

                var theorder = (from x in context.PurchaseOrderDetails
                                where x.PurchaseOrder.VendorID == vendorid
                                && x.PurchaseOrder.OrderDate==DateTime.Today
                                orderby x.StockItem.Description
                                select new CurrentActiveOrderList
                                {
                                    StockItemID = x.StockItem.StockItemID,
                                    Description = x.StockItem.Description,
                                    QuantityOnHand = x.StockItem.QuantityOnHand,
                                    QuantityOnOrder = x.StockItem.QuantityOnOrder,
                                    ReOrderLevel = x.StockItem.ReOrderLevel,
                                    QuantityToOrder = x.Quantity,
                                    Price = Math.Round(x.PurchasePrice, 2)
                                });
                return theorder.ToList();
            }
        }
       public void Update_ItemsFromCurrentOrder(int vendorid, List<CurrentActiveOrderList> CurrentActiveOrderList)
        {
            using (var context = new eToolsContext())
            {
                var order= context.PurchaseOrders
                    .Where(or => or.VendorID == vendorid && or.OrderDate == null)
                    .Select(or => or).FirstOrDefault();            

                var details = (from c in context.PurchaseOrderDetails
                               where c.PurchaseOrderID == order.PurchaseOrderID
                               select c.PurchaseOrderDetailID).ToList();                                  

                decimal sub = 0;
                foreach (CurrentActiveOrderList item in CurrentActiveOrderList)
                {
                    foreach( var id in details)
                    {
                        PurchaseOrderDetail temp = context.PurchaseOrderDetails.Find(id);
                        if(temp.StockItemID==item.StockItemID)
                        {
                            temp.StockItemID = item.StockItemID;
                            temp.Quantity = item.QuantityToOrder;
                            temp.PurchasePrice = item.Price;
                            sub += (item.Price) * (decimal)(item.QuantityToOrder);
                        }                       
                    }                     
  
                }
                order.SubTotal = Math.Round(sub, 2);
                order.TaxAmount = Math.Round((sub * (decimal)0.05), 2);
                context.SaveChanges();               
            }
            }
        public void Place_ToPurchaseOrder( int vendorid, List<CurrentActiveOrderList> CurrentActiveOrderList )
        {
            using (var context = new eToolsContext())
            {
                var order = context.PurchaseOrders
                    .Where(or => or.VendorID == vendorid && or.OrderDate == null)
                    .Select(or => or).FirstOrDefault();

                Update_ItemsFromCurrentOrder(vendorid, CurrentActiveOrderList);
                order.OrderDate = DateTime.Today;
                order.PurchaseOrderNumber = (from c in context.PurchaseOrders
                                             select c.PurchaseOrderNumber).Max() + 1;


                var orderdetailsid = order.PurchaseOrderDetails.
                    Select(de => de.PurchaseOrderDetailID).ToList();

                var stockitem = context.StockItems.Where(st => st.VendorID == vendorid )
                    .Select(st => st).ToList();
               
                foreach (CurrentActiveOrderList item in CurrentActiveOrderList)
                {                  
                     foreach (var stock in stockitem)
                        {
                            if (item.StockItemID == stock.StockItemID)
                            {
                                stock.QuantityOnOrder = stock.QuantityOnOrder + item.QuantityToOrder;
                            }                                               
                    }
                }              
                context.SaveChanges();
            }
        }
      public void Delete_CurrentOpenPurchaseOrderDetils(int vendorid)
        {
            using (var context = new eToolsContext())
            {
                var order= context.PurchaseOrders
                        .Where(c => c.VendorID == vendorid && c.OrderDate == DateTime.Today).
                        Select(c => c).FirstOrDefault();
                if(order!=null)

                {
                    throw new Exception("purchase Order has been already placed. placed order can not be deleted.");

                }
                else
                {
                    var exist = context.PurchaseOrders
                       .Where(c => c.VendorID == vendorid && c.OrderDate == null).
                       Select(c => c).FirstOrDefault();                  
                    
                    
                        var temp = (from c in context.PurchaseOrderDetails
                                    where c.PurchaseOrderID == exist.PurchaseOrderID
                                    select c).ToList();
                        foreach (var item in temp)
                        {
                            context.PurchaseOrderDetails.Remove(item);
                        }
                        context.PurchaseOrders.Remove(exist);
                    }             
            
                context.SaveChanges();
            }
        }
    }
}
