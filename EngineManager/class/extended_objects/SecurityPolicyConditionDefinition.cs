using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class SecurityPolicyConditionDefinition
    {
        public SecurityPolicyConditionDefinition()
        {
            this.SubConditions = new List<SecurityPolicyConditionDefinition>();
        }

        public string ID { get; set; }
        public string Question { get; set; }
        public List<SecurityPolicyConditionDefinition> SubConditions { get; set; }

        public int Order { get; internal set; }

        internal SecurityPolicyCondition GetObject(DBTable t)
        {
            var condition = new SecurityPolicyCondition
            {
                Order = Order,
                Question = t.SecurityPolicyQuestions.Where(q => q.ID == Question).FirstOrDefault()
            };

            foreach (var scd in SubConditions)
            {
                condition.SubConditions.Add(scd.GetObject(t));
            }

            return condition;
        }
    }
}
