using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppSecurity.Entities;

#region Additional Namespaces
using AppSecurity.BLL;
using AppSecurity.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace eToolsWebsite
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This page is public access

            //if (!IsPostBack)
            //{
            //    if (!Request.IsAuthenticated)
            //    {
            //        Response.Redirect("~/Account/Login.aspx");
            //    }
            //    else
            //    {
            //        if (!User.IsInRole(SecurityRoles.WebsiteAdmins))
            //        {
            //            Response.Redirect("~/Account/Login.aspx");
            //        }
            //    }
            //}

        }

        protected void UserListViewODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            // TODO: Note the use of this event to create the ApplicationUserManager.
            //       It is needed because there is NO parameterless constructor on the class.
            e.ObjectInstance = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
    }
}