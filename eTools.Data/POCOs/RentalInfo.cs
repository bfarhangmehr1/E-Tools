using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class RentalInfo
    {
        int RentalID { get; set; }

        public int CustomerID { get; set; }

        public int EmployeeID { get; set; }

        public int? CouponID { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TaxAmount { get; set; }
        //public DateTime RentalDate { get; set; }
        
        public string PaymentType { get; set; }

        public string CreditCard { get; set; }
    }
}
