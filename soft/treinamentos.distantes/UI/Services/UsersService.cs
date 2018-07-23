using App.Models;
using ORM.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service
{
    public class UsersService
    {
        UserDB userRepository = new UserDB();
        public User InsertUser(User user)
        {
            return userRepository.InsertUser(user);
        }
        public User findUserByID(string ID)
        {
            return userRepository.FindUserByID(ID);
        }
        public User findUserByLoginAndPassword(string login, string password)
        {
            return userRepository.FindUserByLoginAndPassword(login, password);
        }
        public List<User> ListActiveUsers()
        {
            return userRepository.ListActiveUsers();
        }
        public List<User> ListNotActiveUsers()
        {
            return userRepository.ListNotActiveUsers();
        }
        public void DeactivateUsersByID(List<string> IDs)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>() { { "active", "false" } };
            foreach (var id in IDs)
            {
                userRepository.UpdateUserByID(id, properties);
            }
        }
        public void UpdateUserPasswordByID(string ID, string password)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>() { { "password", password } };
            
            userRepository.UpdateUserByID(ID, properties);
        }

        public void AddCsvUsers(string csv)
        {
            string[] users = csv.Split(',');
            int count = 0;
            User u = new User();
            foreach (var user in users)
            {
                if (count == 0)
                    u.login = user;
                else if (count == 1)
                    u.password = user;
                else if (count == 2)
                    u.name = user;
                else if (count == 3)
                    u.email = user;

                count++;
                if (count == 4)
                {
                    User userExistent = userRepository.FindUserByLoginAndPassword(u.login, u.password);
                    u.adm = "N";
                    if(userExistent == null)
                        userRepository.InsertUser(u);
                    u.ID = null;
                    count = 0;
                }
            }

        }

        public void UpdateUserByID(string ID, Dictionary<string, object> properties)
        {
            foreach(var property in properties)
                userRepository.UpdateUserByID(ID, new Dictionary<string, object>() { { property.Key, property.Value } });
        }
    }
}
