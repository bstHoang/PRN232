using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week1_DI.Models;

namespace Week1_DI.DataAccess
{
    internal interface ICategoryDA
    {
        List<Category> GetCategories();//ham nay lay categories tu 1 nguon dl nao do
    }
}
