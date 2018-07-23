using App.Models;
using Domain.Core;
using MongoDB.Driver;
using ORM.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORM.Plataforma
{
    public class LayoutDB : ConfigDB
    {
        public Layout InsertLayout(Layout Layout)
        {
            return Context.Insert<Layout>(Layout);
        }

        public Layout FindLayoutByID(string ID)
        {
            FilterDefinition<Layout> filter = Builders<Layout>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<Layout>(filter);
        }

        public List<Layout> ListActiveLayouts()
        {
            FilterDefinition<Layout> filter = Builders<Layout>.Filter.Where(s => s.active == true);
            return Context.GetDocuments<Layout>(filter);
        }
        public List<Layout> ListNotActiveLayouts()
        {
            FilterDefinition<Layout> filter = Builders<Layout>.Filter.Where(s => s.active == false);
            return Context.GetDocuments<Layout>(filter);
        }
        public Layout UpdateLayoutByID(string ID, Dictionary<string, object> properties)
        {
            FilterDefinition<Layout> filter = Builders<Layout>.Filter.Where(u => u.ID == ID);
            Context.Update<Layout>(filter, properties);
            return Context.GetDocument<Layout>(filter);
        }
        public void DeleteLayoutByID(string ID)
        {
            FilterDefinition<Layout> filter = Builders<Layout>.Filter.Where(u => u.ID == ID);
            Context.Delete<Layout>(filter);
        }
    }
}