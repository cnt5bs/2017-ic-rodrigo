using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Hosting;

namespace ORM.Configuracao
{
    public class Connection
    {
        public string GetUrl()
        {
            
            string path = HostingEnvironment.ApplicationPhysicalPath + "/Config/DB.json";
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path))["ConnectionUrl"].Value<string>();
            
        }
    }
}
