using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class OpenOrder
    {
        // PurchaseOrders
        public int POID { get; set; }
        public int? PONumber { get; set; }
        public DateTime? PODate { get; set; }

        // Vendors
        public string VendorName { get; set; }
        public string VendorPhone { get; set; }
    }
}
