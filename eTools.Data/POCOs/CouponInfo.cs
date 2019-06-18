using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CouponInfo
    {
        public int CouponID { get; set; }
        public string CouponCode { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DiscountPercent { get; set; }
        public bool IsValid
        {
            get
            {
                return (DateTime.Now >= StartDate && DateTime.Now < EndDate);
            }
        }
    }
}
