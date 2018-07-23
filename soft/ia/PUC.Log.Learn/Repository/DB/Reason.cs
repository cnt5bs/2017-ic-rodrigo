using Repository.Learn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class Reason
    {
        Dictionary<string, double> formula = new Dictionary<string, double>();

        public KeyValuePair<string, double> getBiggerProbabilityGroup(List<GroupInderectUnderstanding> groups)
        {
            foreach (var item in groups)
            {
                if(!formula.ContainsKey(item.group.FirstOrDefault().value.ToString()))
                    formula.Add(item.group.FirstOrDefault().value.ToString(), item.percentage);
                else
                    formula[item.group.FirstOrDefault().value.ToString()] = formula[item.group.FirstOrDefault().value.ToString()] + item.percentage;
            }
            
            double max = formula.Max(u=>u.Value);
            return formula.Where(u => u.Value == max).FirstOrDefault();
        }
    }
}
