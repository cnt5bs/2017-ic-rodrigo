using App.Models;
using ORM.Conteudo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Services
{
    public class PagesService
    {
        PagesDB PagesRepository = new PagesDB();
        public Pages InsertPages(Pages Pages)
        {
            return PagesRepository.InsertPages(Pages);
        }
        public Pages findPagesByID(string ID)
        {
            Pages Pages = PagesRepository.FindPagesByID(ID);
            return Pages;
        }
        public List<Pages> ListActivePages()
        {
            return PagesRepository.ListActivePages();
        }
        public List<Pages> ListNotActivePages()
        {
            return PagesRepository.ListNotActivePages();
        }
        public void UpdatePagesByID(string ID, Dictionary<string, object> properties)
        {
            PagesRepository.UpdatePagesByID(ID, properties);
        }
        public void DeactivatePagesByID(List<string> IDs)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>() { { "active", "false" } };
            foreach (var id in IDs)
            {
                PagesRepository.UpdatePagesByID(id, properties);
            }
        }
    }
}