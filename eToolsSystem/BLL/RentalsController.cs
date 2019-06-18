using eTools.Data.Entities;
using eTools.Data.POCOs;
using eToolsSystem.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eToolsSystem.BLL
{
    [DataObject]
    public class RentalsController
    {
        //Search for customers 
        public List<Customer> List_Customers()
        {
            using(var context = new eToolsContext())
            {
                return context.Customers.OrderBy(x => x.LastName).ToList();
            }
        }

        //find by phone
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> Get_Customers_by_PhoneNumber(string phonenumber)
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where( x => x.ContactPhone.Contains(phonenumber)).Select(x => x).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> Get_Customers_by_LastName(string LastName)
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.LastName.Contains(LastName)).Select(x => x).ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> Get_Customers_by_FirstName(string FirstName)
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.FirstName.Contains(FirstName)).Select(x => x).ToList();
            }
        }

        //get by postal code
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Customer> Get_Customers_by_PostalCost(string PostalCode)
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.PostalCode.Contains(PostalCode)).Select(x => x).ToList();
            }
        }


        //Get Equipment Description For Drop Down List

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SelectedList> Get_RentalEquipment_Descriptions()
        {
            using (var context = new eToolsContext())
            {
                    var results = from x in context.RentalEquipments
                                  orderby x.Description
                                  select new SelectedList
                                  {
                                      ID = x.RentalEquipmentID,
                                      Label = x.Description
                                  };
                    return results.ToList();
            }
        }

        //Get Avalible Equipment by Equipment Type ID
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RentalEquipment> Get_RentalEquipment_by_TypeID(string equipmentID)
        {
            using (var context = new eToolsContext())
            {
                return context.RentalEquipments.Where(x => x.RentalEquipmentID.Equals(equipmentID)).Select(x => x).ToList();
            }
        }







        //


    }
}
