using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week1_DI.DataAccess;
using Week1_DI.Models;

namespace Week1_DI.Services
{
    internal class CategoryServicesVer1 : ICategoryServices
    {
        ICategoryDA _categoryDA;

        public CategoryServicesVer1(ICategoryDA categoryDA)
        {
            _categoryDA = categoryDA;
        }

        List<Category> ICategoryServices.GetCategories()
        {
            //logic nghiep vu (Ver1) goi o day
           return _categoryDA.GetCategories();
        }
    }
}
