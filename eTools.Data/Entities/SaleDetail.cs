namespace eTools.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SaleDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SaleDetail()
        {
            SaleRefundDetails = new HashSet<SaleRefundDetail>();
        }
        [Key]
        public int SaleDetailID { get; set; }

        public int SaleID { get; set; }

        public int StockItemID { get; set; }

        [Column(TypeName = "money")]
        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public bool Backordered { get; set; }

        public DateTime? ShippedDate { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual StockItem StockItem { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SaleRefundDetail> SaleRefundDetails { get; set; }
    }
}
