namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ReturnedOrderDetail
    {
        [Key]
        public int ReturnedOrderDetailID { get; set; }

        public int ReceiveOrderID { get; set; }

        public int? PurchaseOrderDetailID { get; set; }

        [StringLength(50, ErrorMessage = "ItemDescription accepts a maximum of 50 characters")]
        public string ItemDescription { get; set; }

        public int Quantity { get; set; }

        [StringLength(50, ErrorMessage = "Reason accepts a maximum of 50 characters")]
        public string Reason { get; set; }

        [StringLength(50, ErrorMessage = "VendorStockNumber accepts a maximum of 50 characters")]
        public string VendorStockNumber { get; set; }

        public virtual PurchaseOrderDetail PurchaseOrderDetail { get; set; }

        public virtual ReceiveOrder ReceiveOrder { get; set; }
    }
}
