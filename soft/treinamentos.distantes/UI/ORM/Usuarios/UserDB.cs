using App.Models;
using Domain.Core;
using MongoDB.Driver;
using ORM.Configuracao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ORM.Usuarios
{
    public class UserDB : ConfigDB
    {
        public User InsertUser(User user)
        {
            return Context.Insert<User>(user);
        }

        public User FindUserByID(string ID)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(s => s.ID == ID);
            return Context.GetDocument<User>(filter);
        }

        public User FindUserByLoginAndPassword(string login, string password)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(s => s.login == login && s.password == password && s.active == true);
            return Context.GetDocument<User>(filter);
        }

        public List<User> ListActiveUsers()
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(s => s.active == true);
            return Context.GetDocuments<User>(filter);
        }
        public List<User> ListNotActiveUsers()
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(s => s.active == false);
            return Context.GetDocuments<User>(filter);
        }
        public User UpdateUserByID(string ID, Dictionary<string, object> properties)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(u => u.ID == ID);
            Context.Update<User>(filter, properties);
            return Context.GetDocument<User>(filter);
        }
        public void DeleteUserByID(string ID)
        {
            FilterDefinition<User> filter = Builders<User>.Filter.Where(u => u.ID == ID);
            Context.Delete<User>(filter);
        }
    }
}