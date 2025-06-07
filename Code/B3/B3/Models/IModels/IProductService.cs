using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Models.IModels
{
    internal interface IProductService
    {
        List<Product> GetAllProducts();
    }
}
