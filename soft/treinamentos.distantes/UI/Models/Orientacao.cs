using System.Collections.Generic;

namespace App.Models
{
    public class Orientacao : Base
    {
        public bool temQuestionario { get; set; }
        public string nome { get; set; }
        public string path { get; set; }
        public bool incorporacao { get; set; }
        public List<Questionario> questionario { get; set; }
        
    }
}