using System.Collections.Generic;

namespace App.Models
{
    public class Post : Base
    {
        public string nome { get; set; }
        public string subtitulo { get; set; }
        public string descricao { get; set; }
        public string observacao { get; set; }
        public List<Orientacao> Orientacao { get; set; }
        public List<Pages> pages { get; set; }
        public List<Post> correlatedPosts { get; set; }
        public int ordem { get; set; }
    }
}