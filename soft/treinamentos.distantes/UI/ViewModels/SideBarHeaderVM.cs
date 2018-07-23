using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class SideBarHeaderVM : Base
    {
        public List<Post> posts { get; set; }
    }
}