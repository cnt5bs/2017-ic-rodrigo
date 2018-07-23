using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Pages : Base
    {
        public string name { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public bool hasOrientacao { get; set; } = false;
        public string OrientacaoPath { get; set; } = "";
        public List<string> imagePaths { get; set; }
        public int ordem { get; set; }
    }
}