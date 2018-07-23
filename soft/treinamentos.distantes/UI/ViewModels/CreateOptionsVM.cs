using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class CreateOptionsVM : Base
    {
        public Post post { get; set; }
        public string questionName { get; set; }
        public List<Option> options { get; set; }
    }
}