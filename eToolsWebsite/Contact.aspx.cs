using AppSecurity.BLL;
using AppSecurity.DAL;
using AppSecurity.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using eTools.Data.POCOs;

namespace eToolsWebsite
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This page is public access

            //// Test code to check Staff login
            //if (!IsPostBack)
            //{
            //    if (!Request.IsAuthenticated)
            //    {
            //        Response.Redirect("~/Account/Login.aspx");
            //    }
            //    else
            //    {
            //        if (!User.IsInRole(SecurityRoles.Staff))
            //        {
            //            Response.Redirect("~/Account/Login.aspx");
            //        }
            //    }
            //}

        }
       
    }
}