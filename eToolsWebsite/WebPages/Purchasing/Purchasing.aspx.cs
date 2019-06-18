using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
using AppSecurity.BLL;
using AppSecurity.DAL;
using eTools.Data.POCOs;
using eToolsSystem.BLL;
using Microsoft.AspNet.Identity.EntityFramework;
using eTools.Data.Entities;
using System.Globalization;
#endregion

namespace eToolsWebsite.WebPages
{
    public partial class Purchasing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
            // Only users with Purchasing Role can use this page
            if (!IsPostBack)
            {
               

                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.Purchasing))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }                   
                }               
            }           
        }    

        protected void GetUserName_Click(object sender, EventArgs e)
        {
            //grap the username from security (User)
           
            //obtain the employee information for this username
            MessageUserControl.TryRun(() =>
            {
                string username = User.Identity.Name;
                EmployeeDisplayName.Text = username;
                //standard look call to a controller
                //connect to the ApplicationUserManager
                ApplicationUserManager secmgr = new ApplicationUserManager(
                  new UserStore<ApplicationUser>(new ApplicationDbContext()));
                EmployeeInfo info = secmgr.User_GetEmployee(username);              
                EmployeeName.Text = info.FullName;
            });
        }
        protected void VendorFetch_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            string username = User.Identity.Name;
            ApplicationUserManager secmgr = new ApplicationUserManager(
             new UserStore<ApplicationUser>(new ApplicationDbContext()));
            EmployeeInfo info = secmgr.User_GetEmployee(username);

            if (VendorDLL.SelectedIndex == 0)
            {
                MessageUserControl.ShowInfo("Please select Vendor to retrive information.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                   
                    VendorComtroller sysmgr = new VendorComtroller();
                    Vendor results = sysmgr.Vendor_Get(int.Parse(VendorDLL.SelectedValue));
                    Name.Text = results.VendorName;
                    Phone.Text = null;
                    Address.Text = null;
                    City.Text = null;
                    Province.Text = null;
                    PostalCode.Text = null;
                });
            }          
           
            List<CurrentActiveOrderList> list = new List<CurrentActiveOrderList>();
            MessageUserControl.TryRun(() =>
            {
                int employeeid = info.EmployeeID;
                int vendorid = int.Parse(VendorDLL.SelectedValue);
                for (int rowindex = 0; rowindex < OrderList.Rows.Count; rowindex++)
                {
                    CurrentActiveOrderList temp = new CurrentActiveOrderList();
                  
                    temp.StockItemID = int.Parse((OrderList.Rows[rowindex].FindControl("StockItemID") as Label).Text);
                    temp.Description = (OrderList.Rows[rowindex].FindControl("Description") as Label).Text;
                    temp.QuantityOnHand = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnHand") as Label).Text);
                    temp.QuantityOnOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnOrder") as Label).Text);
                    temp.ReOrderLevel = int.Parse((OrderList.Rows[rowindex].FindControl("ReOrderLevel") as Label).Text);
                    temp.QuantityToOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityToOrder") as TextBox).Text);
                    temp.Price =decimal.Parse((OrderList.Rows[rowindex].FindControl("Price") as TextBox).Text,NumberStyles.Currency);
                    list.Add(temp);
                }
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                List<CurrentActiveOrderList> result = sysmgr.List_StockItemsForSuggestedNewOrder(vendorid, employeeid);
                OrderList.DataSource = result;
                OrderList.DataBind();

                StockItemController sus = new StockItemController();
                List<VendorStockItemsList> result1 = sus.List_VendorStockItemsForCurrentActiveOrder(vendorid, result);
                VendorStockItemList.DataSource = result1;
                VendorStockItemList.DataBind();

                PurchaseOrderController order = new PurchaseOrderController();
                PurchaseOrder result2 = order.Order_GetByVendor(vendorid);
                GSTID.Text = string.Format("{0:C2}", result2.TaxAmount);
                SubTotalID.Text = string.Format("{0:C2}", result2.SubTotal);
                TotalID.Text = string.Format("{0:C2}", result2.Total);
            });     
                          
        }
        protected void ItemSelectionList_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            MessageUserControl.Visible = true;

            if (int.Parse(VendorDLL.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("you must first select vendor name to display Current Active Order and Vendor Stock Item for adding to Purchase Order.");
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    int vendorid = int.Parse(VendorDLL.SelectedValue);
                    int stockitemid = int.Parse(e.CommandArgument.ToString());
                    PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                    sysmgr.Add_LinItemToCurrentActivePurchaseOrdderList(vendorid, stockitemid);
                    Refresh_Displays();

                    VendorComtroller vendorinfo = new VendorComtroller();
                    Vendor results = vendorinfo.Vendor_Get(int.Parse(VendorDLL.SelectedValue));
                    Name.Text = results.VendorName;
                    Phone.Text = results.Phone;
                    Address.Text = results.Address;
                    City.Text = results.City;
                    Province.Text = results.Province;
                    PostalCode.Text = results.PostalCode;                 

                },"Adding Item to Current Order","Item successfully added to the list.");
            }           
        } 
        protected void UpdateOrder_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            List<CurrentActiveOrderList> list = new List<CurrentActiveOrderList>();
            int vendorid = int.Parse(VendorDLL.SelectedValue);

            if (OrderList.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("you must first select vendor name to display Current Active Order for updating Current Purchase Order.");
                VendorStockItemList.DataSource = null;
                VendorStockItemList.DataBind();
            }
            if (int.Parse(VendorDLL.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("you must first select vendor name to display Current Active Order for updating Current Purchase Order.");
            }
            else
            {
                for (int rowindex = 0; rowindex < OrderList.Rows.Count; rowindex++)
                {
                    CurrentActiveOrderList temp = new CurrentActiveOrderList();
                   
                    temp.StockItemID = int.Parse((OrderList.Rows[rowindex].FindControl("StockItemID") as Label).Text);
                    temp.Description = (OrderList.Rows[rowindex].FindControl("Description") as Label).Text;
                    temp.QuantityOnHand = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnHand") as Label).Text);
                    temp.QuantityOnOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnOrder") as Label).Text);
                    temp.ReOrderLevel = int.Parse((OrderList.Rows[rowindex].FindControl("ReOrderLevel") as Label).Text);
                    temp.QuantityToOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityToOrder") as TextBox).Text);
                    temp.Price = decimal.Parse((OrderList.Rows[rowindex].FindControl("Price") as TextBox).Text, NumberStyles.Currency);
                    list.Add(temp);
                }
                MessageUserControl.TryRun(() =>
                {

                    PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                    sysmgr.Update_ItemsFromCurrentOrder(vendorid, list);
                    Refresh_Displays();  

                }, " Updating Item to Current Order", " Items successfully updated. ");

            }
       }
        protected void DeleteOrder_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            if (int.Parse(VendorDLL.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("there is no active order to delete. select vendor to display current order.");
            }

            if (OrderList.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("there is no active order to delete. select vendor to display current order.");
                VendorStockItemList.DataSource = null;
                VendorStockItemList.DataBind();
            }
            else
            {
               
                MessageUserControl.TryRun(() =>
                {
                    int vendorid = int.Parse(VendorDLL.SelectedValue);
                    PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                    sysmgr.Delete_CurrentOpenPurchaseOrderDetils(vendorid);
                    OrderList.DataSource =null;
                    OrderList.DataBind();
                    GSTID.Text = null;
                    SubTotalID.Text = null;
                    TotalID.Text = null;
                    VendorDLL.SelectedIndex = 0;
                    Name.Text = null;
                    Phone.Text = null;
                    Address.Text = null;
                    City.Text = null;
                    Province.Text = null;
                    PostalCode.Text = null;                  
                    VendorStockItemList.DataSource = null;
                    VendorStockItemList.DataBind();

                }, " Removing complete items from Current Order", " Items successfully removed from list.");
            }
        }
        protected void PlaceOrder_Click(object sender, EventArgs e)
        {
           
            MessageUserControl.Visible = true;
            List<CurrentActiveOrderList> list = new List<CurrentActiveOrderList>();

            if (OrderList.Rows.Count == 0)
            {
                MessageUserControl.ShowInfo("you must first select vendor name to display Current Active Order.");
                VendorStockItemList.DataSource = null;
                VendorStockItemList.DataBind();
            }
            if (int.Parse(VendorDLL.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("you must first select vendor name to display Current Active Order.");
            }
            else
            {
                for (int rowindex = 0; rowindex < OrderList.Rows.Count; rowindex++)
                {
                    CurrentActiveOrderList temp = new CurrentActiveOrderList();
                    temp.StockItemID = int.Parse((OrderList.Rows[rowindex].FindControl("StockItemID") as Label).Text);
                    temp.Description = (OrderList.Rows[rowindex].FindControl("Description") as Label).Text;
                    temp.QuantityOnHand = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnHand") as Label).Text);
                    temp.QuantityOnOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityOnOrder") as Label).Text);
                    temp.ReOrderLevel = int.Parse((OrderList.Rows[rowindex].FindControl("ReOrderLevel") as Label).Text);
                    temp.QuantityToOrder = int.Parse((OrderList.Rows[rowindex].FindControl("QuantityToOrder") as TextBox).Text);
                    temp.Price = decimal.Parse((OrderList.Rows[rowindex].FindControl("Price") as TextBox).Text, NumberStyles.Currency);
                    list.Add(temp);
                }
                MessageUserControl.TryRun(() =>
                {
                    int vendorid = int.Parse(VendorDLL.SelectedValue);
                    PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                    sysmgr.Place_ToPurchaseOrder(vendorid, list);
                    List<CurrentActiveOrderList> info = sysmgr.list_PlacedCurerentActiveOrder(vendorid);
                    OrderList.DataSource = info;
                    OrderList.DataBind();

                    PurchaseOrderController order = new PurchaseOrderController();
                    PurchaseOrder result2 = order.CompleteOrder_GetByVendor(vendorid);
                    GSTID.Text = string.Format("{0:C2}", result2.TaxAmount);
                    SubTotalID.Text = string.Format("{0:C2}", result2.SubTotal);
                    TotalID.Text = string.Format("{0:C2}", result2.Total);

                }," Placing order", " Order placed sucessfully.");
            }
        }      
        protected void Clear_Click(object sender, EventArgs e)
        {
            MessageUserControl.Visible = false;
            if (int.Parse(VendorDLL.SelectedValue) == 0)
            {
                MessageUserControl.ShowInfo("there are no Current Order and StockItem list to clear. ");
                MessageUserControl.ShowInfo("");
            }
            GSTID.Text = "";
            SubTotalID.Text = "";
            TotalID.Text = "";
            VendorDLL.SelectedIndex = 0;
            Name.Text = null;
            Phone.Text = null;
            Address.Text = null;
            City.Text = null;
            Province.Text = null;
            PostalCode.Text = null;
            OrderList.DataSource = null;
            OrderList.DataBind();
            VendorStockItemList.DataSource = null;
            VendorStockItemList.DataBind();
        }
        protected void Refresh_Displays()
        {
            int vendorid = int.Parse(VendorDLL.SelectedValue);
            PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
            List<CurrentActiveOrderList> info = sysmgr.list_AddedCurerentActiveOrder(vendorid);
            OrderList.DataSource = info;
            OrderList.DataBind();

            StockItemController sus = new StockItemController();
            List<VendorStockItemsList> stock = sus.List_VendorStockItemsForCurrentActiveOrder(vendorid, info);
            VendorStockItemList.DataSource = stock;
            VendorStockItemList.DataBind();

            PurchaseOrderController order = new PurchaseOrderController();
            PurchaseOrder result2 = order.Order_GetByVendor(vendorid);
            GSTID.Text = string.Format("{0:C2}", result2.TaxAmount);
            SubTotalID.Text = string.Format("{0:C2}", result2.SubTotal);
            TotalID.Text = string.Format("{0:C2}", result2.Total);
        }
        public void OrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {          
            OrderList.PageIndex = e.NewPageIndex;
            Refresh_Displays();           
        }
        protected void OrderList_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageUserControl.Visible = true;
            int vendorid = int.Parse(VendorDLL.SelectedValue);
            int stockitmeid = int.Parse((OrderList.Rows[OrderList.SelectedIndex].FindControl("StockItemID") as Label).Text);

            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderDetailController sysmgr = new PurchaseOrderDetailController();
                sysmgr.Remove_CurrentActivePurchaseOrdder(vendorid, stockitmeid);
                Refresh_Displays();
            }, " Removing Item from Current Order", " Item successfully removed from list.");
        }
    }
}