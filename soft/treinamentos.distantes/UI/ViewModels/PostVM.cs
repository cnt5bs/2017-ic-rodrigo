using App.Models;
using System.Collections.Generic;

namespace UI.ViewModels
{
    public class PostVM : Base
    {
        public List<Post> posts { get; set; }
        public Post post { get; set; }
        public int pageIndex { get; set; } = 0;
    }
}