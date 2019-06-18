using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.DTOs
{
    public class SalesReceiptItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal SellingPrice { get; set; }
        public int Qty { get; set; }
        public decimal SubTotal { get; set; }
    }
}
