using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace App.Models
{
    public class Pergunta : Base
    {
        public string name { get; set; }
        public string idPrototipo { get; set; }
        public string question { get; set; }
        public List<Option> options { get; set; }
        public List<Resposta> resposta { get; set; }
        public Resposta ultimaResposta { get; set; }
        public TimeSpan tempoParaResponder { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime horaAberturaPergunta { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime horaFechamentoPergunta { get; set; }

        public string correctAnswerID { get; set; }
    }
}