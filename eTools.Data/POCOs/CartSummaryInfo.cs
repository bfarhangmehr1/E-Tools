using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CartSummaryInfo
    {
        const decimal GST_RATE = 0.05M;

        public int ShoppingCartID { get; set; }
        public int ProductCount { get; set; }
        public int ItemCount { get; set; }
        public decimal Subtotal { get; set; }

        public CouponInfo DiscountCoupon { get; set; }

        // For a discount offered at the time of sale, you charge GST/HST on the net amount (the sale price minus the discount).
        // See: https://www.canada.ca/en/revenue-agency/services/forms-publications/publications/rc4022-general-information-gst-hst-registrants.html
        public decimal Discount
        {
            get
            {
                return (DiscountCoupon != null && DiscountCoupon.IsValid) ? Subtotal * DiscountCoupon.DiscountPercent / 100.0M : 0M;
            }
        }

        // For a discount offered at the time of sale, you charge GST/HST on the net amount (the sale price minus the discount).
        // See: https://www.canada.ca/en/revenue-agency/services/forms-publications/publications/rc4022-general-information-gst-hst-registrants.html
        public decimal GST
        {
            get
            {
                return (Subtotal - Discount) * GST_RATE;
            }
        }

        public decimal Total
        {
            get
            {
                return Subtotal - Discount + GST;
            }
        }

    }
}
