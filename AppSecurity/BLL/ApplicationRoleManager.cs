using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using AppSecurity.DAL;
using AppSecurity.Entities;
using AppSecurity.POCOs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
#endregion

namespace AppSecurity.BLL
{
    [DataObject]
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        private ApplicationUserManager UserManager { get; set; }

        public ApplicationRoleManager() :
            base(new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {
            //needed to add this to get UserManager in ListAllRoles() to have a value.
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }
        public ApplicationRoleManager(ApplicationUserManager userManager) :
            base(new RoleStore<IdentityRole>(new ApplicationDbContext()))
        {
            UserManager = userManager;
        }

        public void AddDefaultRoles()
        {
            foreach (string roleName in SecurityRoles.DefaultSecurityRoles)
            {
                // Check if it exists
                if (!Roles.Any(r => r.Name == roleName))
                {
                    this.Create(new IdentityRole(roleName));
                }
            }
        }

        #region UserRole Administration
        public List<RoleName> ListAllRoleNames()
        {
            //used on administration page to load the checkboxlist
            //  on the User Panel
            //needs POCO RoleName
            var results = from role in Roles.ToList()
                          where role.Name != SecurityRoles.RegisteredUsers
                          select new RoleName
                          {
                              Name = role.Name
                          };
            return results.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<RoleProfile> ListAllRoles()
        {
            //used on administration page to load the Listview
            //  on the Role Panel
            //needs POCO RoleProfile
            //ListView is based on the POCO; NOT an entity
            var results = from role in Roles.ToList()
                          select new RoleProfile
                          {
                              RoleId = role.Id,
                              RoleName = role.Name,
                              UserNames = role.Users.Select(r => UserManager.FindById(r.UserId).UserName)
                          };
            return results.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void AddRole(RoleProfile role)
        {
            //used on administration page to add from the Listview
            //  on the Role Panel
            //Listview is based on POCO
            if (!this.RoleExists(role.RoleName))
            {
                this.Create(new IdentityRole(role.RoleName));
            }
            else
            {
                throw new Exception("Creation failed. " + role.RoleName + " already exists.");
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void DeleteRole(RoleProfile role)
        {
            //used on administration page to delete from the Listview
            //  on the Role Panel
            //ListView is based on POCO
            var existing = this.FindById(role.RoleId);
            if (existing.Users.Count() == 0)
            {
                this.Delete(this.FindById(role.RoleId));
            }
            else
            {
                throw new Exception("Delete failed. " + role.RoleName + " has existing users. Reassign users first.");
            }
        }
        #endregion
    }
}
