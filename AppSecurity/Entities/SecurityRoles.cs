using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSecurity.Entities
{
    public static class SecurityRoles
    {
        public const string WebsiteAdmins = "WebsiteAdmins";
        public const string RegisteredUsers = "RegisteredUsers";
        public const string Staff = "Staff";

        // Purchasing must only allow authenicated users within the Purchasing Role to have access to the subsystem.
        public const string Purchasing = "Purchasing";
        // Receiving must only allow authenicated users within the Receiving Role to have access to the subsystem.
        public const string Receiving = "Receiving";
        // Sales must only allow authenicated users within the Sales Role to have access to the subsystem. BUT THEN
        // Employees can view the catalog of products even when they are not logged on.
        public const string Sales = "Sales";
        // Rentals must only allow authenicated users within the Rentals Role to have access to the subsystem
        public const string Rentals = "Rentals";

        public static List<string> DefaultSecurityRoles
        {
            get
            {
                List<string> value = new List<string>();
                value.Add(WebsiteAdmins);
                value.Add(RegisteredUsers);
                value.Add(Staff);
                value.Add(Purchasing);
                value.Add(Receiving);
                value.Add(Sales);
                value.Add(Rentals);
                return value;
            }
        }
    }
}
