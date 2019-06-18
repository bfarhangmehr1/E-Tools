using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Namespaces
using eToolsWebsite.UserControls;
using eToolsSystem.BLL;
using eTools.Data.Entities;
using eTools.Data.POCOs;
using AppSecurity.BLL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using AppSecurity.DAL;
#endregion

namespace eToolsWebsite.WebPages.Rentals
{
    public partial class ReturnRental : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void CustomerRentalsLookupButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CustomerPhoneLookupTextbox.Text) && string.IsNullOrEmpty(CustomerPhoneLookupTextbox.Text))
            {
                MessageUserControl.ShowInfo("Enter Search Data", "You must enter at least a  partial phone number and or email address to search for a customer");
            }
            else
            {
                //GetCustomersByPhoneAndEmail(CustomerPhoneLookupTextbox.Text, CustomerEmailLookupTextbox.Text);  

                MessageUserControl.TryRun(() => {
                    RentalController sysmgr = new RentalController();
                    List<Customer> results =
                    sysmgr.List_RentalsByCustomerPhone(CustomerPhoneLookupTextbox.Text);
                    if (results.Count() == 0)
                    {
                        MessageUserControl.ShowInfo("No results, check your search criteria");
                    }
                    CustomerList.DataSource = results;
                    CustomerList.DataBind();
                });
            }
        }

        
        protected void CustomerSelected_Click(object sender, ListViewCommandEventArgs e)
        {
            int customerID = int.Parse(e.CommandArgument.ToString());

            string EmoployeeUsername = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(EmoployeeUsername);

            EmployeeID.Text = info.EmployeeID.ToString();

            MessageUserControl.TryRun(() => {

                CustomerController sysmgr = new CustomerController();
                List<Customer> results = sysmgr.List_CustomersByID(customerID);
                CustomerInfoGridview.DataSource = results;
                CustomerInfoGridview.DataBind();

                RentalController rntlmgr = new RentalController();
                List<Rental> rentals = rntlmgr.Get_RentalsByCusomterSearch(customerID);
                RentalListListview.DataSource = rentals;
                RentalListListview.DataBind();

            }, "Customer Selected", "Please select a rental by this customer");

            CustomerList.DataBind();

            CustomerID.Text = customerID.ToString();

            //list rentals and display them same as did customers
        }


        protected void RentalSelected_Click(object sender, ListViewCommandEventArgs e)
        {
            int rentalID = int.Parse(e.CommandArgument.ToString());

            RentalID.Text = rentalID.ToString();

            MessageUserControl.TryRun(() => {

                RentalController rntlmgr = new RentalController();
                List<RentalDetail> rentals = rntlmgr.Get_RentalDetailsByRentalID(rentalID);
                RentalListListview.DataSource = rentals;
                RentalListListview.DataBind();

            }, "Customer Selected", "Please select a rental by this customer");

            CustomerList.DataBind();

            //list rentals and display them same as did customers


        }

        protected void PayNowButton_Click(object sender, EventArgs e)
        {
            string paymentType = PaymentType.Text;
            MessageUserControl.TryRun(() => {
                RentalController rental = new RentalController();
                rental.PayNow(int.Parse(RentalID.Text), paymentType);
            }, "Customer Selected", "Please select a rental by this customer");
        }

        public void updateTotals()
        {
            int subtotal = int.Parse(TextBCheckoutSubtoalTextboxox2.Text);
            int GST = int.Parse(CheckoutGSTTextbox.Text);
            int discountAmount = int.Parse(CheckoutDiscountAmountTextbox.Text);
            int total = int.Parse(CheckoutTotalTextbox.Text);
            
            MessageUserControl.TryRun(() =>
            {
                RentalController rental = new RentalController();

               // rental.UpdateRentalTotals();


            }, "Updated Totals", "Payment totals have been updated");


        }





    }//class
}