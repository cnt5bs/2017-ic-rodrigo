using App.Models;
using Domain.Core;
using MongoDB.Driver;
using ORM.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.ORM.Conteudo
{
    public class PrototipoDB : ConfigDB
    {
        public Prototipo InsertQuestionario(Prototipo Questionario)
        {
            return Context.Insert<Prototipo>(Questionario);
        }

        public Prototipo FindQuestionarioByID(string ID)
        {
            FilterDefinition<Prototipo> filter = Builders<Prototipo>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<Prototipo>(filter);
        }
    }
}