using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Repository.Configuracao
{
    public class Connection
    {
        public string GetUrl(string appPath)
        {
            string path = appPath + "/Config/DB.json";
            return JsonConvert.DeserializeObject<JObject>(File.ReadAllText(path))["ConnectionIA"].Value<string>();
        }
    }
}
