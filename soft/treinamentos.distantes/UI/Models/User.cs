using System.Collections.Generic;

namespace App.Models
{
    public class User : Base
    {
        
        public string login { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string adm { get; set; }
        public List<Post> posts { get; set; } = new List<Post>();
    }
}