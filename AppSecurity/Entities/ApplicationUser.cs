using System.Threading.Tasks;

#region Additional Namespaces
using System.Security.Claims;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using AppSecurity.BLL;
#endregion

namespace AppSecurity.Entities
{
    public class ApplicationUser : IdentityUser
    {

        //Add nullable fields for linking user data (ie employee, customer) to security data
        public int? EmployeeID { get; set; }
        public int? CustomerID { get; set; }

        public ClaimsIdentity GenerateUserIdentity(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = manager.CreateIdentity(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            return Task.FromResult(GenerateUserIdentity(manager));
        }
    }
}
