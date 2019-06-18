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
    public partial class CreateNewRental : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (RentalID == null || RentalID.Text == "")
            {
                RentalID.Text = "0";
            }
        }

        //Lookup

        //check if either or both fields are filled in call search methods

        public void SearchForCustomers(object sender, EventArgs e)
        {
            MessageUserControl.ShowInfo(CustomerEmailLookupTextbox.Text + " " + CustomerPhoneLookupTextbox.Text);
            
            if(string.IsNullOrEmpty(CustomerEmailLookupTextbox.Text) && string.IsNullOrEmpty(CustomerPhoneLookupTextbox.Text))
            {
                MessageUserControl.ShowInfo("Enter Search Data", "You must enter at least a  partial phone number and or email address to search for a customer");
            }
            else
            {
                //GetCustomersByPhoneAndEmail(CustomerPhoneLookupTextbox.Text, CustomerEmailLookupTextbox.Text);  

                MessageUserControl.TryRun(() => {
                    CustomerController sysmgr = new CustomerController();
                    List<Customer> results =
                    sysmgr.List_CustomersByPhoneAndEmail(CustomerEmailLookupTextbox.Text, CustomerPhoneLookupTextbox.Text);
                    if (results.Count() == 0)
                    {
                        MessageUserControl.ShowInfo("No results, check your search criteria");
                    }
                    CustomerList.DataSource = results;
                    CustomerList.DataBind();
                });

            }
        }

        public int Get_CouponByID(int couponID)
        {
            int exists = 0;
            MessageUserControl.TryRun(() => {

                RentalController sysmgr = new RentalController();
                List<Coupon> results = sysmgr.List_CouponByID(couponID);
                
                if(results != null)
                {
                    exists = 1;
                }
                else
                {
                    exists = 2;
                }
                
            }, "Customer Selected", "Selected customer for new rental");

            return exists;

           //need to pass it when submitting //same with creditcard 

        }

        //Select customer clicked 
        //CustomerSelected_Click
        public void CustomerSelected_Click(object sender, ListViewCommandEventArgs e)
        {
            int customerID = int.Parse(e.CommandArgument.ToString());
                       
            string EmoployeeUsername = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(EmoployeeUsername);
            
            EmployeeID.Text = info.EmployeeID.ToString();

            MessageUserControl.TryRun(() => {
                
                CustomerController sysmgr = new CustomerController();
                List <Customer> results = sysmgr.List_CustomersByID(customerID);
                CustomerInfoGridview.DataSource = results;
                CustomerInfoGridview.DataBind();

            }, "Customer Selected", "Selected customer for new rental");

            //CustomerList.DataBind();

            CustomerID.Text = customerID.ToString();
        }

        protected void EquipmentSearchButton_Click(object sender, EventArgs e)
        {
            //search for all equipment with the selected description
            //get by description and throw in listview
            MessageUserControl.ShowInfo("Search for Equipment: " + EquipmentDescriptionSearch.Text);
           
            MessageUserControl.TryRun(() => {
                RentalController sysmgr = new RentalController();
                List<RentalEquipment> results =
                sysmgr.List_RentalEquipmentByDescriptionSearch(EquipmentDescriptionSearch.Text);
                if (results.Count() == 0)
                {
                    MessageUserControl.ShowInfo("No results, check your search criteria");
                }
                AvalibleEquipmentListview.DataSource = results;
                AvalibleEquipmentListview.DataBind();
            });
        }



        public void EquipmentSelected_Click(object sender, ListViewCommandEventArgs e)
        {
            int equipmentID = int.Parse(e.CommandArgument.ToString());
            
            int employeeID = int.Parse(EmployeeID.Text);
            int customerID = int.Parse(CustomerID.Text);
            int rentalID = int.Parse(RentalID.Text);

            if (employeeID == 0 || customerID == 0)
            {
                MessageUserControl.ShowInfo("No Customer Selected", "Please select a customer before adding equipment to be rented");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    RentalController rentalcontroller = new RentalController();
                    if (rentalID == 0)
                    {
                        rentalID = rentalcontroller.Create_Rental(employeeID, customerID, int.Parse(RentalID.Text));
                    }
                    rentalcontroller.Create_RentalDetails(rentalID, equipmentID);
                    List<RentalEquipmentInfo> renting = rentalcontroller.List_RentingEquipment(int.Parse(RentalID.Text));

                    EquipmentBeingRentedListview.DataSource = renting;
                    EquipmentBeingRentedListview.DataBind();

                }, "Equipment Info Added", "Added Equipment to rental");

                RentalID.Text = rentalID.ToString();
            }
        }

        public void EquipmentRemoved_Click(object sender, ListViewCommandEventArgs e)
        {
            int equipmentID = int.Parse(e.CommandArgument.ToString());

            

        }
        
        protected void Submit_Click(object sender, EventArgs e)
        {
            //when submit mark equipmnet as not avalible in rental details where rental details rentalid == rentalID

            if (CreditCardTextbox != null && RentalID != null)
            {
                int couponID = int.Parse(CouponID.Text);
                int creditcard = int.Parse(CreditCardTextbox.Text);


                MessageUserControl.TryRun(() =>
                {
                    RentalController sysmgr = new RentalController();

                    RentalInfo rentalinfo = new RentalInfo();

                    MessageUserControl.ShowInfo(rentalinfo.EmployeeID.ToString());

                    //RentalDetail details = new RentalDetail();

                    //update rental where rentalID = rentalID add coupon and creditcard

                }, "Completed Rental", "The rental has been created");
            }
            else
            {
                MessageUserControl.ShowInfo("Credit Card Is Required", "Please enter a credit card number to complete the rental");
            }
            
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            int rentalID = int.Parse(RentalID.Text);

            MessageUserControl.TryRun(() => {
                
                RentalController rentalcontroller = new RentalController();
                
                //remove thing
                rentalcontroller.Cancel_Rental(rentalID);


            }, "Rental Canceled", "Cancelled the current rental");
            
            //refresh

        }

        protected void CouponCheck_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {

                RentalController rentalcontroller = new RentalController();

                //remove thing
                rentalcontroller.Cancel_Rental(int.Parse(CouponID.Text));


            }, "Rental Canceled", "Cancelled the current rental");
        }
    }//class
}