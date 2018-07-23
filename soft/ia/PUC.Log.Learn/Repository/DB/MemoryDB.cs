using System;
using System.Collections.Generic;
using Repository.Configuracao;
using Parameters;
using Domain.Core;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using Repository.Learn;

namespace Repository
{
    public class MemoryDB : ConfigDB
    {
        public MemoryDB() : base(Configuration.appPath) { }

        public void InsertMemory(Understand memory)=>Context.Insert("Memory_COLLECTION", memory);

        public List<JObject> ListAllMemories() => Context.GetDocuments<JObject>("Memory_COLLECTION", Builders<JObject>.Filter.Where(s => true));

        public void InsertIndirectUnderstanding(object responsible, string fact, object environment)
        {
            foreach (var responsibleProp in responsible.GetType().GetProperties())
            {
                Context.Insert("Combinations_COLLECTION", new InderectUnderstanding(responsibleProp.GetValue(responsible), fact){ environment = environment });
            }
        }

        public void InsertIndirectUnderstanding(object responsible, object fact, object environment)
        {

            foreach (var responsibleProp in responsible.GetType().GetProperties())
            {
                foreach (var factProp in fact.GetType().GetProperties())
                {
                    
                    Context.Insert("Combinations_COLLECTION", new InderectUnderstanding(responsibleProp.GetValue(responsible), factProp.GetValue(fact)) { environment = environment });
                }
            }
        }

        
        
    }

}
