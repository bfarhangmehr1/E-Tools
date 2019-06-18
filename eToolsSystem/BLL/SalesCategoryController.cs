using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eTools.Data.POCOs;
using eToolsSystem.DAL;
#endregion

namespace eToolsSystem.BLL
{
    [DataObject(true)]
    public class SalesCategoryController
    {

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<SalesCategory> ListAllCategories()
        {
            using (var context = new eToolsContext())
            {
                // Get all categories with non-discontinued products
                List<SalesCategory> result =
                    (from p in context.StockItems
                    where !p.Discontinued
                    group p by p.Category into grp
                    orderby grp.Key.Description
                    select new SalesCategory
                    {
                        CategoryID = grp.Key.CategoryID,
                        CategoryName = grp.Key.Description,
                        ProductCount = grp.Count()
                    }).ToList();

                // Add "All" category with total number of products
                SalesCategory allCategory = 
                    new SalesCategory
                    {
                        CategoryID = 0,
                        CategoryName = "All",
                        ProductCount = (from r in result select r.ProductCount).Sum()
                    };
                result.Insert(0, allCategory);

                return result;
            }
        }
    }
}
