using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region More Namespaces
using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System.ComponentModel;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject]
    public class UnorderedPurchaseItemCartController
    {
        /// <summary>
        /// Placeholder (empty) content
        /// </summary>
        /// <returns></returns>
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<UnorderedPurchaseItemCart> UPIC_List()
        {
            return new List<UnorderedPurchaseItemCart>();
        }

        /// <summary>
        /// Retrieves all unordered purchase item entries for the current session
        /// </summary>
        /// <param name="_CartIDs"></param>
        /// <returns></returns>
        public List<UnorderedPurchaseItemCart> UPIC_ListByID(List<int> _CartIDs)
        {
            using (var context = new eToolsContext())
            {
                List<UnorderedPurchaseItemCart> UPICs = new List<UnorderedPurchaseItemCart>();

                if (_CartIDs != null)
                {
                    foreach (var i in _CartIDs)
                    {
                        var upic = (
                            from x in context.UnorderedPurchaseItemCart
                            where x.CartID == i
                            select x)
                            .FirstOrDefault();

                        UPICs.Add(upic);
                    }
                }

                return UPICs;
            }
        }
        
        /// <summary>
        /// Inserts an unordered purchase item to the database
        /// </summary>
        /// <param name="_upic"></param>
        public int UPIC_Add(UnorderedPurchaseItemCart _upic)
        {
            using (var context = new eToolsContext())
            {
                // Stage instance for physical insertion
                context.UnorderedPurchaseItemCart.Add(_upic);

                // Commit the changes to the database
                context.SaveChanges();

                // Output of the generated identity ID
                return _upic.CartID;
            }
        }

        /// <summary>
        /// Removes an unordered purchase item from the database
        /// </summary>
        /// <param name="_upic"></param>
        public void UPIC_Delete(int _cartID)
        {
            using (var context = new eToolsContext())
            {
                var existingUPIC = context.UnorderedPurchaseItemCart.Find(_cartID);

                // Validate provided record
                if (existingUPIC == null)
                {
                    throw new Exception("Provided UPIC not found on file");
                }
                else
                {
                    context.UnorderedPurchaseItemCart.Remove(existingUPIC);
                    context.SaveChanges();
                }
            }
        }
    }
}
