using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parameters;
using Newtonsoft.Json.Linq;

namespace Machine
{
    public static class Configuration
    {
        public static void InsertAppPath(string appPath)
        {
            if (Parameters.Configuration.appPath != appPath)
                Parameters.Configuration.appPath = appPath;
        }
        public static string GetCurrentAppPath()
        {
            return Parameters.Configuration.appPath;
        }
    }
}
