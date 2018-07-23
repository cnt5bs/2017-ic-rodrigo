using Domain;
using Domain.Core;
using MongoDB.Driver;

namespace Repository.Configuracao
{
    public class ConfigDB
    {
        public ConfigDB(string appPath)
        {
            Configuration configuration = new Configuration();
            Connection conn = new Connection();
            configuration.dbName = "CMS_VIDEOS";
            configuration.mongoUrl = new MongoUrl(conn.GetUrl(appPath));
            Context.Initializer(configuration);
        }
        ~ConfigDB()
        {
            Context.Dispose();
        }
    }
}