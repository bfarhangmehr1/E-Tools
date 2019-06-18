using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.DTOs
{
    public class SalesReceipt
    {
        public int SaleID { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentTypeDesc { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Decimal SubTotal { get; set; }
        public int? CouponId { get; set; }
        public string CouponCode { get; set; }
        public int DiscountPercent { get; set; }
        public Decimal DiscountAmount { get; set; }
        public Decimal TaxAmount { get; set; }
        public Decimal TotalAmount { get; set; }

        public List<SalesReceiptItem> Items { get; set; }
    }
}
