using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class OpenOrderDetail
    {
        // PurchaseOrderDetails
        public int POID { get; set; }
        public int PODetailID { get; set; }
        public int QtyOrdered { get; set; }
        public int StockItemID { get; set; }

        // StockItems
        public string Description { get; set; }

        // New fields
        public int QtyOutstanding { get; set; }
        public int? QtyReceived { get; set; }
        public int? QtyReturned { get; set; }
        public string ReturnReason { get; set; }
    }
}
