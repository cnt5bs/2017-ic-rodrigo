using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace App.Models
{
    public class Questionario : Base
    {
        public string prototypeID { get; set; }
        public User owner { get; set; }
        public List<Pergunta> perguntas { get; set; }
        public string nome { get; set; }
        public TimeSpan tempoParaResponderQuestionario { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dataIniciado { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime dataFinalizado { get; set; }
    }
}