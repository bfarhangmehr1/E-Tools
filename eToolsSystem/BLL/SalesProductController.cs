using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.POCOs;
using eToolsSystem.DAL;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject(true)]
    public class SalesProductController
    {

        // retrieve list of products in a category or all products if categoryid = 0
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SalesProductInfo> ProductListByCategory(int categoryID)
        {
            using (var context = new eToolsContext())
            {
                var qry = context.StockItems.Where(p => !p.Discontinued);
                // if categoryID is specified, filter for that category
                if (categoryID > 0)
                {
                    qry = qry.Where(p => p.CategoryID == categoryID);
                }
                
                var result = qry.OrderBy(p => p.Description).Select(p => 
                    new SalesProductInfo
                    {
                        ProductID = p.StockItemID,
                        ProductDescription = p.Description,
                        SellingPrice = p.SellingPrice,
                        QtyOnHand = p.QuantityOnHand,
                        QtyOnOrder = p.QuantityOnOrder,
                        PendingOrderDate = 
                            (
                            from po in p.PurchaseOrderDetails
                            where !po.PurchaseOrder.Closed
                            select po.PurchaseOrder.OrderDate
                            ).Min()
                    });

                return result.ToList();
            }
        }
    }
}
