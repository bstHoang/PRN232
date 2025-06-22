using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientHR.Models
{
    internal class CreateEmployeeModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int PositionId { get; set; }
    }
}
