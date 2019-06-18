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
    public class PurchaseOrderController
    {
        /// <summary>
        /// Retrieves all outstanding (unclosed) orders as a usable list
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<OpenOrder> OpenOrder_List()
        {
            using (var context = new eToolsContext())
            {
                var OpenOrders = (
                    from x in context.PurchaseOrders
                    where x.Closed == false
                    select new OpenOrder
                    {
                        POID = x.PurchaseOrderID,
                        PONumber = x.PurchaseOrderNumber,
                        PODate = x.OrderDate,
                        VendorName = x.Vendor.VendorName,
                        VendorPhone = x.Vendor.Phone
                    })
                    .ToList();

                return OpenOrders;
            }
        }

        /// <summary>
        /// Retrieves an outstanding order (with a specified ID)
        /// </summary>
        /// <param name="_purchaseOrderID"></param>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public OpenOrder OpenOrder_FindByID(int _purchaseOrderID)
        {
            using (var context = new eToolsContext())
            {
                var openOrder = (
                    from x in context.PurchaseOrders
                    where x.PurchaseOrderID == _purchaseOrderID
                    select new OpenOrder
                    {
                        POID = x.PurchaseOrderID,
                        PONumber = x.PurchaseOrderNumber,
                        PODate = x.OrderDate,
                        VendorName = x.Vendor.VendorName,
                        VendorPhone = x.Vendor.Phone
                    })
                    .FirstOrDefault();

                return openOrder;
            }
        }
        public PurchaseOrder Order_GetByVendor(int vendorid)

        {
            using (var context = new eToolsContext())
            {
                return context.PurchaseOrders.
                    Where(x => x.VendorID.Equals(vendorid) && x.OrderDate == null).Select(x => x).FirstOrDefault();
            }
        }

        public PurchaseOrder CompleteOrder_GetByVendor(int vendorid)

        {
            using (var context = new eToolsContext())
            {
                return context.PurchaseOrders.
                    Where(x => x.VendorID.Equals(vendorid) && x.OrderDate == DateTime.Today).Select(x => x).FirstOrDefault();
            }
        }


    }
}
