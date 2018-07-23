using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Models
{
    public class Categoria : Base
    {
        public string nome { get; set; }
        public string description { get; set; }
        public List<Assunto> assuntos { get; set; }
    }
}