using App.Models;
using System.Collections.Generic;

namespace UI.ViewModels
{
    public class DashboardVM : Base
    {
        public List<Post> posts { get; set; }
    }
}