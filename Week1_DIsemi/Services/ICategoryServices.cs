using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week1_DI.Models;

namespace Week1_DI.Services
{
    internal interface ICategoryServices
    {
        List<Category> GetCategories();
    }
}
