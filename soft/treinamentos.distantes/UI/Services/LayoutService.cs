using App.Models;
using ORM.Plataforma;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Services
{
    public class LayoutService
    {
        LayoutDB LayoutRepository = new LayoutDB();
        public Layout InsertLayout(Layout Layout)
        {
            return LayoutRepository.InsertLayout(Layout);
        }
        public Layout findLayoutByID(string ID)
        {
            return LayoutRepository.FindLayoutByID(ID);
        }
        public List<Layout> ListActiveLayouts()
        {
            return LayoutRepository.ListActiveLayouts();
        }
        public List<Layout> ListNotActiveLayouts()
        {
            return LayoutRepository.ListNotActiveLayouts();
        }
        public void UpdateLayoutByID(string ID, Dictionary<string, object> properties)
        {
            LayoutRepository.UpdateLayoutByID(ID, properties);
        }
        public void DeactivateLayoutsByID(List<string> IDs)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>() { { "active", "false" } };
            foreach (var id in IDs)
            {
                LayoutRepository.UpdateLayoutByID(id, properties);
            }
        }
    }
}