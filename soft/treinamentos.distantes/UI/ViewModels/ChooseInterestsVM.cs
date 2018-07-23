using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class ChooseInterestsVM : Base
    {
        public List<Post> posts { get; set; }
        public List<Post> userPosts { get; set; }
        public Post postBind { get; set; }
    }
}