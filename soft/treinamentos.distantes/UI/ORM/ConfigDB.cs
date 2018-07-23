using Domain;
using Domain.Core;
using MongoDB.Driver;
using System.Web.Hosting;

namespace ORM.Configuracao
{
    public class ConfigDB
    {
        public ConfigDB()
        {
            Configuration configuration = new Configuration();
            Connection conn = new Connection();
            configuration.dbName = "CMS_Orientacao";
            configuration.mongoUrl = new MongoUrl(conn.GetUrl());
            Context.Initializer(configuration);
            
        }
        ~ConfigDB()
        {
            Context.Dispose();
        }
    }
}