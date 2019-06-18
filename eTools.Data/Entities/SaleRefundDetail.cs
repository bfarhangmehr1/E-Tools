namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaleRefundDetail
    {
        [Key]
        public int SaleRefundDetailID { get; set; }

        public int SaleRefundID { get; set; }

        public int SaleDetailID { get; set; }

        [Column(TypeName = "money")]
        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public virtual SaleDetail SaleDetail { get; set; }

        public virtual SaleRefund SaleRefund { get; set; }
    }
}
