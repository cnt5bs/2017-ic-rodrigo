using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class SingleExamAnalyticsVM : Base
    {
        public List<Post> posts { get; set; }
        public List<Post> userPosts { get; set; }
        public Post post { get; set; }
        public Dictionary<Pergunta, Resposta> resposta_usuario { get; set; }
        public Dictionary<Pergunta, Resposta> resposta_esperadas { get; set; }
        public Dictionary<Pergunta, Resposta> resposta_corretas { get; set; }
    }
}