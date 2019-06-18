using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSecurity.POCOs
{
    public class RoleProfile
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public IEnumerable<string> UserNames { get; set; }
    }
}
