using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW1.Config
{
    public class ReadJson
    {
        public PassConfig LoadConfig()
        {
            var json = File.ReadAllText("config.json");
            return JsonConvert.DeserializeObject<PassConfig>(json);
        }
    }
}
