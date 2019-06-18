using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class RentalDetailsInfo
    {
        public int RentalDetailID { get; set; }
        public int RentalID { get; set; }
        public int RentalEquipmentID { get; set; }
        public double Days { get; set; }
        public decimal DailyRate { get; set; }
        public string ConditionOut { get; set; }
        public string ConditionIn { get; set; }        
        public decimal DamageRepairCost { get; set; }
        public string Comments { get; set; }
        public bool Paid { get; set; }
    }
}
