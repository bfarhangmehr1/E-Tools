using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class SalesProductInfo
    {
        public int ProductID { get; set; }
        public String ProductDescription { get; set; }
        public Decimal SellingPrice { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyOnOrder { get; set; }
        public DateTime? PendingOrderDate { get; set; }
    }
}
