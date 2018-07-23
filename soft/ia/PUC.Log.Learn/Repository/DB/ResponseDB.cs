using Parameters;
using Repository.Configuracao;
using System.Collections.Generic;
using System.Linq;
using Domain.Core;
using Newtonsoft.Json.Linq;
using MongoDB.Driver;
using Repository.Learn;
using Newtonsoft.Json;

namespace Repository
{
    public class ResponseDB : ConfigDB
    {
        public ResponseDB() : base(Configuration.appPath)
        {
        }

        public void InsertResponse(JObject response)
        {
            Context.Insert("Responses_COLLECTION", response);
        }

        public JObject GetConclusionFromResponsible(object responsible, object environment)
        {
            JObject conclusions = new JObject();
            foreach (var responsibleProp in responsible.GetType().GetProperties())
            {
                object propertyValue = responsibleProp.GetValue(responsible);

                FilterDefinition<InderectUnderstanding> filter = Builders<InderectUnderstanding>.Filter.Where(k => k.key == propertyValue && k.environment == environment);
                List<InderectUnderstanding> result = Context.GetDocuments<InderectUnderstanding>("Combinations_COLLECTION", filter);

                conclusions[responsibleProp.Name] = JsonConvert.SerializeObject(result);
                conclusions[responsibleProp.Name + "_" + responsibleProp.GetValue(responsible).ToString() + "%"] = JsonConvert.SerializeObject(result);
                conclusions[responsibleProp.Name + "_" + responsibleProp.GetValue(responsible).ToString() + "%%"] = JsonConvert.SerializeObject(result.GroupBy(u => (u.value)).Select(u => new { valor = u, valorContagem = u.Count(), total = result.Count(), porcentagem = (u.Count() + 0.0) / result.Count() }));
            }
            return conclusions;
        }
        public JObject GetUniqueConclusionFromResponsible(object responsible, object environment, List<string> relevantNames)
        {
            JObject conclusions = new JObject();
            KeyValuePair<object, double> choice_chance;
            choice_chance = new KeyValuePair<object, double>("", 0.0);
            List<GroupInderectUnderstanding> biggerProbabilityGroups = new List<GroupInderectUnderstanding>();
            foreach (var responsibleProp in responsible.GetType().GetProperties())
            {
                object propertyValue = responsibleProp.GetValue(responsible);
                FilterDefinition<InderectUnderstanding> filter;
                if (relevantNames != null)
                    filter = Builders<InderectUnderstanding>.Filter.Where(k => k.key == propertyValue && k.environment == environment && relevantNames.Contains(k.value));
                else
                    filter = Builders<InderectUnderstanding>.Filter.Where(k => k.key == propertyValue && k.environment == environment);
                List<InderectUnderstanding> result = Context.GetDocuments<InderectUnderstanding>("Combinations_COLLECTION", filter);
                if (result.Count > 0)
                {
                    double maxPercentage = result.GroupBy(u => (u.value))
                                                                         .Select(u => new GroupInderectUnderstanding()
                                                                         {
                                                                             group = u,
                                                                             count = u.Count(),
                                                                             total = result.Count(),
                                                                             percentage = (u.Count() + 0.0) / result.Count()
                                                                         }).Select(u => u.percentage).Max();

                    biggerProbabilityGroups.Add(result.GroupBy(u => (u.value))
                                                                             .Select(u => new GroupInderectUnderstanding()
                                                                             {
                                                                                 group = u,
                                                                                 count = u.Count(),
                                                                                 total = result.Count(),
                                                                                 percentage = (u.Count() + 0.0) / result.Count()
                                                                             }).Where(u => u.percentage == maxPercentage).FirstOrDefault());
                }


            }
            
            KeyValuePair<string, double> biggerResponseGroup = new Reason().getBiggerProbabilityGroup(biggerProbabilityGroups);
            
            JObject biggerResponseGroupConsidered = new JObject();
            
             biggerResponseGroupConsidered["group"] = biggerResponseGroup.Key;
             biggerResponseGroupConsidered["percentage"] = biggerResponseGroup.Value;
            
            return biggerResponseGroupConsidered;
        }
        public JObject GetUniqueConclusionFromResponsible(object responsible, object environment)
        {
            return GetUniqueConclusionFromResponsible(responsible, environment, null);
        }
    }
}
