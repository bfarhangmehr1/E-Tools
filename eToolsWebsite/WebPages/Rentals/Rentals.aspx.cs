using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.Entities;
#endregion

namespace eToolsWebsite.WebPages.Rentals
{
    public partial class Rentals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Only users with Rentals Role can use this page
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.Rentals))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }

        }

        protected void RetturnRentalButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReturnRental.aspx");
        }

        protected void CreateNewRentalButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateNewRental.aspx");
        }

        










    }
}