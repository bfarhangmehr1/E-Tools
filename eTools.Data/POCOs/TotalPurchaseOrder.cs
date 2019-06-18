using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
 public   class TotalPurchaseOrder
    {
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
       
    }
}
