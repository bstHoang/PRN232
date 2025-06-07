using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week1_DI.Models;

namespace Week1_DI.DataAccess
{
    internal class CategoryDASQLServer : ICategoryDA
    {
        //trien khai cu the lay cateogries tu nguon sqlserver.
        List<Category> ICategoryDA.GetCategories() 
        {
            ProductCatalogContext context = new ProductCatalogContext();
            return context.Categories.ToList();
        }
    }
}
