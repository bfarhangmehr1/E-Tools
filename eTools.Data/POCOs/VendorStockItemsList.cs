using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
   public class VendorStockItemsList
    {
        public int StockItemID { get; set; }
        public string Description { get; set; }

        public int QuantityOnHand { get; set; }
        public int QuantityOnOrder { get; set; }
        public int ReOrderLevel { get; set; }
        public decimal PurchasePrice { get; set; }
        //New field
        public int Buffer { get; set; }
       
    }
}
