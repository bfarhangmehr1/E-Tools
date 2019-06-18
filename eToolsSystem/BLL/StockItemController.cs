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
  public  class StockItemController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<VendorStockItemsList> List_VendorStockItemsForCurrentActiveOrder( int vendorid, List<CurrentActiveOrderList> CurrentActiveOrderList)
        {
            using (var context = new eToolsContext())
            {
                var stockList = (from c in context.StockItems
                                 where c.VendorID == vendorid
                                 orderby c.Description
                                 select new VendorStockItemsList
                                 {
                                     StockItemID = c.StockItemID,
                                     Description = c.Description,
                                     QuantityOnHand = c.QuantityOnHand,
                                     QuantityOnOrder = c.QuantityOnOrder,
                                     ReOrderLevel = c.ReOrderLevel,
                                     Buffer = Math.Abs((c.QuantityOnOrder + c.QuantityOnHand) - c.ReOrderLevel),
                                     PurchasePrice = Math.Round(c.PurchasePrice, 2)
                                 });

                var resultsStock = stockList.ToList().
                            Where(st => !CurrentActiveOrderList.Any(c => c.StockItemID == st.StockItemID)).
                            OrderBy(st => st.Description).
                            Select(st => st);
                return resultsStock.ToList();        

            }
            //using (var context = new eToolsContext())
            //{

            //    var exists = context.PurchaseOrders.
            //       Where(x => x.VendorID.Equals(vendorid) && x.OrderDate == null).Select(x => x).FirstOrDefault();

            //    //decimal sub = 0;
            //    //decimal total = 0;            
            //    if (exists != null)
            //    {
            //        var results = (from x in context.PurchaseOrderDetails
            //                       where x.PurchaseOrder.VendorID == exists.VendorID &&
            //                      x.PurchaseOrder.PurchaseOrderNumber == null && x.PurchaseOrder.OrderDate == null
            //                       orderby x.StockItem.Description
            //                       select new CurrentActiveOrderList
            //                       {
            //                           StockItemID = x.StockItem.StockItemID,
            //                           Description = x.StockItem.Description,
            //                           QuantityOnHand = x.StockItem.QuantityOnHand,
            //                           QuantityOnOrder = x.StockItem.QuantityOnOrder,
            //                           ReOrderLevel = x.StockItem.ReOrderLevel,
            //                           QuantityToOrder = x.Quantity,
            //                           Price = x.StockItem.PurchasePrice
            //                       });
            //        var resultsStock = context.StockItems.
            //                Where( st => !results.ToList().Any(c => c.StockItemID == st.StockItemID)).
            //                OrderBy(st => st.Description).
            //                Select(st => st);

            //        var stockList = (from c in resultsStock.ToList()
            //                         where c.VendorID == vendorid
            //                         orderby c.Description
            //                         select new VendorStockItemsList
            //                         {
            //                             StockItemID = c.StockItemID,
            //                             Description = c.Description,
            //                             QuantityOnHand = c.QuantityOnHand,
            //                             QuantityOnOrder = c.QuantityOnOrder,
            //                             ReOrderLevel = c.ReOrderLevel,
            //                             Buffer = Math.Abs((c.QuantityOnOrder + c.QuantityOnHand) - c.ReOrderLevel),
            //                             PurchasePrice = c.PurchasePrice

            //                         });

            //        return stockList.ToList();
            //    }
            //    else
            //    {
            //        var results = (from x in context.StockItems
            //                       where x.VendorID == vendorid
            //                       && x.ReOrderLevel > (x.QuantityOnHand + x.QuantityOnOrder)
            //                       orderby x.Description
            //                       select new CurrentActiveOrderList
            //                       {
            //                           StockItemID = x.StockItemID,
            //                           Description = x.Description,
            //                           QuantityOnHand = x.QuantityOnHand,
            //                           QuantityOnOrder = x.QuantityOnOrder,
            //                           ReOrderLevel = x.ReOrderLevel,
            //                           QuantityToOrder = 0,
            //                           Price = x.PurchasePrice
            //                       });


            //        var resultsStock = context.StockItems.
            //                                   Where(st => !results.ToList().Any(c => c.StockItemID == st.StockItemID)).
            //                                   OrderBy(st => st.Description).
            //                                   Select(st => st);

            //        var stockList = (from c in resultsStock.ToList()
            //                         where c.VendorID == vendorid
            //                         orderby c.Description
            //                         select new VendorStockItemsList
            //                         {
            //                             StockItemID = c.StockItemID,
            //                             Description = c.Description,
            //                             QuantityOnHand = c.QuantityOnHand,
            //                             QuantityOnOrder = c.QuantityOnOrder,
            //                             ReOrderLevel = c.ReOrderLevel,
            //                             Buffer = Math.Abs((c.QuantityOnOrder + c.QuantityOnHand) - c.ReOrderLevel),
            //                             PurchasePrice = c.PurchasePrice

            //                         });

            //        return stockList.ToList();

            //    }
            //}



        }
    }// eom
}
