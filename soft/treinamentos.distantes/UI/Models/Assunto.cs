using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Assunto : Base
    {
        public string name { get; set; }
        public string description { get; set; }
        public string Questionario { get; set; }
    }
}