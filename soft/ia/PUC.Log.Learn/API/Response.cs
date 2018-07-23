using Newtonsoft.Json.Linq;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Machine
{
    public class Response
    {
        public void InsertResponses(JObject responses)
        {
            ResponseDB response = new ResponseDB();
            response.InsertResponse(responses);
        }
        
        public JObject GetItemResponses(object responsible, object environment)
        {
            ResponseDB response = new ResponseDB();
            return response.GetConclusionFromResponsible(responsible, environment);
        }
        public JObject GetConclusion(object responsible, object environment)
        {
            ResponseDB response = new ResponseDB();
            return response.GetUniqueConclusionFromResponsible(responsible, environment);
        }
        public JObject GetConclusion(object responsible, object environment, List<string> relevantNames)
        {
            ResponseDB response = new ResponseDB();
            return response.GetUniqueConclusionFromResponsible(responsible, environment, relevantNames);
        }
    }
}
