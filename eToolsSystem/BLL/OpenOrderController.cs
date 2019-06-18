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
using System.Data.Entity;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class OpenOrderController
    {
        /// <summary>
        /// Transactional process of an outstanding order:
        /// <para>>> Updates the order details</para>
        /// <para>>> Updates stock item quantity fields for the receive order detail rows</para>
        /// <para>>> Creates a receive order record</para>
        /// <para>>> Creates receive order details and/or returned order details record(s)</para>
        /// <para>>> Updates the outstanding order as closed if fully received</para>
        /// </summary>
        /// <param name="_poID"></param>
        /// <param name="_OODs"></param>
        /// <param name="_UPICs"></param>
        public bool OpenOrder_Receive(int _poID, List<OpenOrderDetail> _OODs, List<int> _UPICs)
        {
            using (var context = new eToolsContext())
            {
                // Transaction process:
                // A) Create a ReceiveOrder record when at least one item is received, returned, or unordered
                // B) Create a new ReceiveOrderDetail record for each item that is received
                // C) Increase stock item QuantityOnHand by the received quantity
                // D) Decrease stock item QuantityOnOrder by the received quantity
                // E) Create a new ReturnedOrderDetail record for each item that is returned
                // F) Check if the PurchaseOrder can be closed
                // G) Create a new ReturnedOrderDetail record for each item that is unordered
                // H) Save all changes

                // All the records that can be created or updated in the transaction processs
                List<ReceiveOrderDetail> ReceiveOrderDetails = new List<ReceiveOrderDetail>();
                List<ReturnedOrderDetail> ReturnedOrderDetails = new List<ReturnedOrderDetail>();
                List<StockItem> StockItems = new List<StockItem>();
                PurchaseOrder purchaseOrder = null;

                // A >>
                ReceiveOrder receiveOrder = new ReceiveOrder(); // Note: ReceiveOrderID is an identity created when ReceiveOrder inserted
                receiveOrder.PurchaseOrderID = _poID;
                receiveOrder.ReceiveDate = DateTime.Now; // Nullable

                // Continue if order details exist
                if (_OODs.Count > 0)
                {
                    bool isOpen = false; // Reverse logic check where order has to be proven to be open (upon iteration through each order detail row)

                    // Process each open order detail
                    foreach (OpenOrderDetail ood in _OODs)
                    {
                        StockItem stockItem = null;
                        int qtyOutstanding = ood.QtyOutstanding;
                        int qtyReceived = (int)ood.QtyReceived;

                        // Check if item was received
                        if (ood.QtyReceived != null && ood.QtyReceived > 0)
                        {
                            // Ensure received qty is not greather than the outstanding qty
                            if (qtyReceived > qtyOutstanding)
                            {
                                throw new Exception("Each received qty must be less than the outstanding qty");
                            }
                            else
                            {
                                // Monitor order state
                                if (qtyReceived < qtyOutstanding)
                                {
                                    isOpen = true;
                                }

                                // B >>
                                ReceiveOrderDetail receiveOrderDetail = new ReceiveOrderDetail(); // Note: ReceiveOrderID is an identity created when ReceiveOrder inserted
                                receiveOrderDetail.PurchaseOrderDetailID = ood.PODetailID;
                                receiveOrderDetail.QuantityReceived = qtyReceived;

                                // Pull stock item from database
                                int stockItemID = ood.StockItemID;
                                stockItem = (
                                    from x in context.StockItems
                                    where x.StockItemID == stockItemID
                                    select x)
                                    .FirstOrDefault();

                                // C >>
                                stockItem.QuantityOnHand += qtyReceived;

                                // D >>
                                stockItem.QuantityOnOrder -= qtyReceived;

                                // Hold created and updated records
                                ReceiveOrderDetails.Add(receiveOrderDetail);
                                StockItems.Add(stockItem);
                            }
                        }
                        else // Item not received
                        {
                            // Monitor order state
                            if (qtyOutstanding > 0)
                            {
                                isOpen = true;
                            }
                        }

                        // Check if item was returned
                        if (ood.QtyReturned != null && ood.QtyReturned > 0)
                        {
                            // Ensure a reason was entered by the user
                            if (string.IsNullOrEmpty(ood.ReturnReason))
                            {
                                throw new Exception("Returned quantities require a reason entry");
                            }
                            else
                            {
                                // Pull stock item from database (if it has not yet been retrieved)
                                if (stockItem == null)
                                {
                                    int stockItemID = ood.StockItemID;
                                    stockItem = (
                                        from x in context.StockItems
                                        where x.StockItemID == stockItemID
                                        select x)
                                        .FirstOrDefault();
                                }

                                // E >>
                                ReturnedOrderDetail returnedOrderDetail = new ReturnedOrderDetail(); // Note: ReceiveOrderID is an identity created when ReceiveOrder inserted
                                returnedOrderDetail.PurchaseOrderDetailID = ood.PODetailID; // Nullable
                                returnedOrderDetail.ItemDescription = stockItem.Description; // Nullable
                                returnedOrderDetail.Quantity = (int)ood.QtyReturned;
                                returnedOrderDetail.Reason =
                                    string.IsNullOrEmpty(ood.ReturnReason)
                                    ? null
                                    : ood.ReturnReason;
                                returnedOrderDetail.VendorStockNumber = null; // Nullable

                                // Hold created record
                                ReturnedOrderDetails.Add(returnedOrderDetail);
                            }
                        }
                    }

                    // F >>
                    if (isOpen == false)
                    {
                        // Pull purchase order from database
                        purchaseOrder = (
                            from x in context.PurchaseOrders
                            where x.PurchaseOrderID == _poID
                            select x)
                            .FirstOrDefault();

                        // Close order
                        purchaseOrder.Closed = true;
                    }
                }

                // Process each unordered purchase item cart (Note: This can happen even if there are no order details for the order)
                if (_UPICs.Count > 0)
                {
                    foreach (int cart in _UPICs)
                    {
                        // Pull upic from database
                        int cartID = cart;
                        var upic = (
                            from x in context.UnorderedPurchaseItemCart
                            where x.CartID == cartID
                            select x)
                            .FirstOrDefault();

                        // G >>
                        ReturnedOrderDetail returnedOrderDetail = new ReturnedOrderDetail(); // Note: ReceiveOrderID is an identity created when ReceiveOrder inserted
                        returnedOrderDetail.PurchaseOrderDetailID = null; // Nullable
                        returnedOrderDetail.ItemDescription = upic.Description; // Nullable
                        returnedOrderDetail.Quantity = upic.Quantity;
                        returnedOrderDetail.Reason = null; // Nullable
                        returnedOrderDetail.VendorStockNumber =
                            string.IsNullOrEmpty(upic.VendorStockNumber)
                            ? null
                            : upic.VendorStockNumber;

                        // Hold created record
                        ReturnedOrderDetails.Add(returnedOrderDetail);
                    }
                }

                // H >>
                context.ReceiveOrders.Add(receiveOrder); // Creates identity ID used by detail records

                foreach (ReceiveOrderDetail rod in ReceiveOrderDetails)
                {
                    // Set detail records receive order ID
                    rod.ReceiveOrderID = receiveOrder.ReceiveOrderID;

                    // Stage detail record
                    context.ReceiveOrderDetails.Add(rod);
                }

                foreach (ReturnedOrderDetail rod in ReturnedOrderDetails)
                {
                    // Set detail record receive order ID
                    rod.ReceiveOrderID = receiveOrder.ReceiveOrderID;

                    // Stage detail record
                    context.ReturnedOrderDetails.Add(rod);
                }

                foreach (StockItem si in StockItems)
                {
                    context.Entry(si).Property(y => y.QuantityOnHand).IsModified = true;
                    context.Entry(si).Property(y => y.QuantityOnOrder).IsModified = true;
                }

                bool closed = false;
                if (purchaseOrder != null) // Purchase order can be closed
                {
                    context.Entry(purchaseOrder).Property(y => y.Closed).IsModified = true;
                    closed = true;
                }

                // Commit transaction
                context.SaveChanges();

                // Return order state
                return closed;
            }
        }

        /// <summary>
        /// Transactional force close of an outstanding order
        /// </summary>
        /// <param name="_poID"></param>
        /// <param name="_forceCloseReason"></param>
        /// <param name="_OODs"></param>
        public void OpenOrder_ForceClose(int _poID, string _forceCloseReason, List<OpenOrderDetail> _OODs)
        {
            using (var context = new eToolsContext())
            {
                // Transaction process:
                // A) Update the purchase order as closed
                // B) Update purchase order notes field with force close reason
                // C) Update QuantityOnOrder for each stock item by subtracting outstanding quantity

                // Ensure a reason was entered by the user
                if (string.IsNullOrEmpty(_forceCloseReason))
                {
                    throw new Exception("A reason is required for a force close of an order");
                }
                else
                {
                    // Pull purchase order from database
                    var purchaseOrder = (
                        from x in context.PurchaseOrders
                        where x.PurchaseOrderID == _poID
                        select x)
                        .FirstOrDefault();

                    // A >>
                    purchaseOrder.Closed = true;
                    context.Entry(purchaseOrder).Property(y => y.Closed).IsModified = true;

                    // B >>
                    purchaseOrder.Notes = _forceCloseReason;
                    context.Entry(purchaseOrder).Property(y => y.Notes).IsModified = true;

                    // C >>
                    List<StockItem> StockItems = new List<StockItem>();
                    foreach (OpenOrderDetail ood in _OODs)
                    {
                        int stockItemID = ood.StockItemID;
                        var stockItem = (
                            from x in context.StockItems
                            where x.StockItemID == stockItemID
                            select x)
                            .FirstOrDefault();

                        stockItem.QuantityOnOrder -= ood.QtyOutstanding;

                        // Update qty on order
                        context.Entry(stockItem).Property(y => y.QuantityOnOrder).IsModified = true;
                    }

                    // Commit changes
                    context.SaveChanges();
                }
            }
        }

    }
}
