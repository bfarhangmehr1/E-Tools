
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace eToolsWebsite.Security
{
    public partial class UserRoleAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Request.IsAuthenticated)
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
                else
                {
                    if (!User.IsInRole(SecurityRoles.WebsiteAdmins))
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void UserListViewODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            // TODO: Note the use of this event to create the ApplicationUserManager.
            //       It is needed because there is NO parameterless constructor on the class.
            e.ObjectInstance = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        protected void RoleNameODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            // TODO: Note the use of this event to create the ApplicationUserManager.
            //       It is needed because there is NO parameterless constructor on the class.
            e.ObjectInstance = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        protected void RoleODS_ObjectCreating(object sender, ObjectDataSourceEventArgs e)
        {
            // TODO: Note the use of this event to create the ApplicationUserManager.
            //       It is needed because there is NO parameterless constructor on the class.
            e.ObjectInstance = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        protected void RefreshAll(object sender, EventArgs e)
        {
            DataBind();
        }

        protected void UserListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            //collect roles the user will be assigned to
            var addtoroles = new List<string>();

            //point to the CheckBox list in the listview InsertTemplate
            var roles = e.Item.FindControl("RoleMemberships") as CheckBoxList;

            //control exists?
            if (roles != null)
            {
                //cycle through the checkbox list
                foreach (ListItem item in roles.Items)
                {
                    if (item.Selected)
                    {
                        addtoroles.Add(item.Value);
                    }
                }
                e.Values["RoleMemberships"] = addtoroles;
            }
        }
    }
}