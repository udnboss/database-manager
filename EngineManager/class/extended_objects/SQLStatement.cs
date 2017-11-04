using System.ComponentModel;

namespace EngineManager
{
    public class SQLStatement : UniqueID
    {
        [Description("Specify the name of the command.")]
        public string Name { get; set; }
        public DBTable Table { get; set; }
        [Description("Applicable to Insert/Delete statements only.")]
        public DBTable TargetTable { get; set; }
        [TypeConverter(typeof(SecurityPolicyListTypeConverter))]
        public SecurityPolicy WherePolicy { get; set; }
        public virtual string GetSyntax() { return ""; }
        public override string ToString()
        {
            return Name;
        }
    }
}
