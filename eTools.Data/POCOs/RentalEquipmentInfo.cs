using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class RentalEquipmentInfo
    {

        public int RentalEquipmentID { get; set; }

        public string Description { get; set; }
        
        public string ModelNumber { get; set; }

        
        public string SerialNumber { get; set; }

        
        public decimal DailyRate { get; set; }

        
        public string Condition { get; set; }

        public bool Available { get; set; }

        public bool Retired { get; set; }


    }
}
