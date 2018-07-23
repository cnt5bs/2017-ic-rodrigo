using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class PagesEditVM : Base
    {
        public List<Pages> pages { get; set; }
        public Post post { get; set; }
    }
}