using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class AnalyticsVM : Base
    {
        public List<Post> posts { get; set; }
        public List<Post> userPosts { get; set; }
        public Dictionary<Post, int> post_nota { get; set; }
        public Dictionary<Post, int> post_esperada { get; set; }
        public List<Post> badResultsPosts { get; set; }
    }
}