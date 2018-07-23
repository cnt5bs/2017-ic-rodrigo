using App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ViewModels
{
    public class QuestionarioVM : Base
    {
        public Post post { get; set; }
        public List<Post> posts { get; set; }
        public Orientacao Orientacao { get; set; }
        public Questionario questionario { get; set; }
        public int questionIndex { get; set; }
        public int pageCurrentIndex { get; set; }
    }
}