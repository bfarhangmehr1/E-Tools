using eTools.Data.Entities;
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
    public class CustomerController
    {
        //List / Search /Get
        public List<Customer> List_CustomersByPhoneAndEmail(string email, string phone)
        {
            using(var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.ContactPhone.Contains(phone) && x.EmailAddress.Contains(email)).Select(x => x).ToList();
            }
        }

        public List<Customer> List_CustomersByID(int CustomerID)
        {
            using (var context = new eToolsContext())
            {
                return context.Customers.Where(x => x.CustomerID.Equals(CustomerID)).Select(x => x).ToList();
            }
        }




    }
}
