using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parameters
{
    public static class Configuration
    {
        public static string appPath { get; set; }

        public static List<string> relevantPropertyNames { get; set; }

        public static Dictionary<string, string> selector_to_selector { get; set; }

        public static Dictionary<string, string> weight { get; set; }

        public static void Configure()
        {

        }
    }
}
