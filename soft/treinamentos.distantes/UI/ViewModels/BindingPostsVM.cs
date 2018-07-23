using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class BindingPostsVM : Base
    {
        public Post post { get; set; }
        public List<Post> allPostsNotBind { get; set; }
        public List<Post> allPostsBind { get; set; }
    }
}