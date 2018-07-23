using App.Models;
using Domain.Core;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORM.Conteudo
{
    public class PagesDB
    {
        public Pages InsertPages(Pages Pages)
        {
            return Context.Insert<Pages>(Pages);
        }

        public Pages FindPagesByID(string ID)
        {
            FilterDefinition<Pages> filter = Builders<Pages>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<Pages>(filter);
        }

        public List<Pages> ListActivePages()
        {
            FilterDefinition<Pages> filter = Builders<Pages>.Filter.Where(s => s.active == true);
            return Context.GetDocuments<Pages>(filter);
        }
        public List<Pages> ListNotActivePages()
        {
            FilterDefinition<Pages> filter = Builders<Pages>.Filter.Where(s => s.active == false);
            return Context.GetDocuments<Pages>(filter);
        }
        public Pages UpdatePagesByID(string ID, Dictionary<string, object> properties)
        {
            FilterDefinition<Pages> filter = Builders<Pages>.Filter.Where(u => u.ID == ID);
            Context.Update<Pages>(filter, properties);
            return Context.GetDocument<Pages>(filter);
        }
        public void DeletePagesByID(string ID)
        {
            FilterDefinition<Pages> filter = Builders<Pages>.Filter.Where(u => u.ID == ID);
            Context.Delete<Pages>(filter);
        }
    }
}