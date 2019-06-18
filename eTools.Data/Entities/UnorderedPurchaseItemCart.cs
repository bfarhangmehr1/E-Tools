namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnorderedPurchaseItemCart")]
    public class UnorderedPurchaseItemCart
    {
        [Key]
        public int CartID { get; set; }

        public int PurchaseOrderID { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(100, ErrorMessage = "Description accepts a maximum of 100 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "VendorStockNumber is required")]
        [StringLength(25, ErrorMessage = "VendorStockNumber accepts a maximum of 25 characters")]
        public string VendorStockNumber { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}
