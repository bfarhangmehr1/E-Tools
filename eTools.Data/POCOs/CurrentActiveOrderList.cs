using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
  public  class CurrentActiveOrderList
    {
        // PurchaseOrderDetails
        //  public int PurchaseOrderDetailID { get; set; }
        // StockItems
        //public int PurchaseOrderDetailID { get; set; }
        public int StockItemID { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public int ReOrderLevel { get; set; }

        // New fields
        public int QuantityToOrder { get; set; }
        public decimal Price { get; set; }

    }
}
