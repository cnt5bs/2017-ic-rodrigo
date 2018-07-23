using App.Models;
using Domain.Core;
using MongoDB.Driver;
using ORM.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORM.Conteudo
{
    public class QuestionarioDB : ConfigDB
    {
        public Questionario InsertQuestionario(Questionario Questionario)
        {
            return Context.Insert<Questionario>(Questionario);
        }

        public Prototipo InsertPrototipo(Prototipo prototipoQuestionario)
        {
            return Context.Insert<Prototipo>(prototipoQuestionario);
        }

        public Questionario FindQuestionarioByID(string ID)
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<Questionario>(filter);
        }
        public Questionario FindQuestionarioByUserIDAndPrototype(string userID, string prototypeID)
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(s => s.prototypeID == prototypeID && s.owner.ID == userID);
            return Context.GetDocument<Questionario>(filter);
        }

        public List<Questionario> ListActiveQuestionarios()
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(s => s.active == true);
            return Context.GetDocuments<Questionario>(filter);
        }
        public List<Questionario> ListActiveQuestionariosByPrototipoID(string prototipoID)
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(s => s.active == true && s.prototypeID == prototipoID);
            return Context.GetDocuments<Questionario>(filter);
        }
        public List<Questionario> ListNotActiveQuestionarios()
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(s => s.active == false);
            return Context.GetDocuments<Questionario>(filter);
        }
        public Questionario UpdateQuestionarioByID(string ID, Dictionary<string, object> properties)
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(u => u.ID == ID);
            Context.Update<Questionario>(filter, properties);
            return Context.GetDocument<Questionario>(filter);
        }
        public void DeleteQuestionarioByID(string ID)
        {
            FilterDefinition<Questionario> filter = Builders<Questionario>.Filter.Where(u => u.ID == ID);
            Context.Delete<Questionario>(filter);
        }
    }
}