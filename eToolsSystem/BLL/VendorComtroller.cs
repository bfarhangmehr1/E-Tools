using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eToolsSystem.DAL;
using System.ComponentModel;
using eTools.Data.POCOs;
#endregion
namespace eToolsSystem.BLL
{
    [DataObject]
    public  class VendorComtroller
    {

        public List<Vendor> Vendor_List()
        {
            using (var context = new eToolsContext())
            {
                return context.Vendors.OrderBy(x=>x.VendorName).ToList();
            }
        }
        public Vendor Vendor_Get(int vendorId)
        {
            using (var context = new eToolsContext())
            {
                return context.Vendors.Find(vendorId);
            }
        }
    }
}
