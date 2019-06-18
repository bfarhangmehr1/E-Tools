using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.Entities;
using eToolsSystem.DAL;
#endregion

namespace eToolsSystem.BLL
{
    public class EmployeeController
    {
        /// <summary>
        /// Finds and returns an employee by their associated ID
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public Employee Employee_Get(int employeeID)
        {
            using (var context = new eToolsContext())
            {
                return context.Employees.Find(employeeID);
            }
        }
    }
}
