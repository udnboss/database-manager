using System.ComponentModel;

namespace EngineManager
{
    public class Validator : UniqueID
    {

        [Browsable(false)]
        public DBTable Table { get; set; }
        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy SecurityPolicy { get; set; }

        public bool Insert { get; set; }
        public bool Update { get; set; }

        internal ValidatorDefinition GetDefinition()
        {
            return new ValidatorDefinition {ID = ID, SecurityPolicy = SecurityPolicy.ID };
        }
    }
}
