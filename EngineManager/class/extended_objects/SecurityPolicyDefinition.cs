using System.Collections.Generic;

namespace EngineManager
{
    public class SecurityPolicyDefinition
    {
        public string ID { get; set; }
        public List<SecurityPolicyConditionDefinition> Conditions { get; set; }

        public SecurityPolicyDefinition()
        {
            this.Conditions = new List<SecurityPolicyConditionDefinition>();
        }

        public string Name { get; set; }

        public SecurityPolicy GetObject(DBTable t)
        {
            var policy = new SecurityPolicy { ID = ID, Name = Name };
            foreach (var d in Conditions) //condition tree for each policy
            {
                policy.Conditions.Add(d.GetObject(t));
            }

            return policy;
        }
    }
}
