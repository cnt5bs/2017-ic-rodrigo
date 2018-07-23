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
    public class PostagensDB : ConfigDB
    {
        public Post InsertPost(Post Post)
        {
            return Context.Insert<Post>(Post);
        }

        public Post FindPostByID(string ID)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<Post>(filter);
        }
        
        public List<Post> ListActivePosts()
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Where(s => s.active == true && s.Orientacao != null);
            return Context.GetDocuments<Post>(filter);
        }
        public List<Post> ListNotActivePosts()
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Where(s => s.active == false);
            return Context.GetDocuments<Post>(filter);
        }
        public Post UpdatePostByID(string ID, Dictionary<string, object> properties)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Where(u => u.ID == ID);
            Context.Update<Post>(filter, properties);
            return Context.GetDocument<Post>(filter);
        }
        public void DeletePostByID(string ID)
        {
            FilterDefinition<Post> filter = Builders<Post>.Filter.Where(u => u.ID == ID);
            Context.Delete<Post>(filter);
        }
    }
}