using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Learn
{
    public class InderectUnderstanding : Base
    {
        public InderectUnderstanding(object key, object value)
        {
            this.key = key;
            this.value = value;
        }
        public object ID { get; set; }
        public object key { get; set; }
        public object value { get; set; }
        public object environment { get; set; }
    }
    public class GroupInderectUnderstanding
    {   
        public IGrouping<object, InderectUnderstanding> group { get; set; }
        public int count { get; set; }
        public double total { get; set; }
        public double percentage { get; set; }
    }
}
