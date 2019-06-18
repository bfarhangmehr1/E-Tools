using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTools.Data.POCOs
{
    public class CartItemInfo
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public String ProductDescription { get; set; }
        public Decimal SellingPrice { get; set; }
        public int QtyOnHand { get; set; }
        public int QtyInCart { get; set; }
        public Decimal TotalPrice {
            get
            {
                return SellingPrice * QtyInCart;
            }
        }
        public bool IsOutOfStock {
            get
            {
                return QtyOnHand <= 0;
            }
        }
        public bool isValid { 
            get
            {
                return (ProductID > 0) && (QtyInCart > 0) && (QtyInCart <= QtyOnHand);
            }
        }

        public override string ToString()
        {
            return string.Format("{0} x {1}, unit price {2:C2}, subtotal {3:C2}", QtyInCart, ProductDescription, SellingPrice, TotalPrice);
        }
    }
}
