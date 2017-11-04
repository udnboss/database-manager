using System.ComponentModel;

namespace EngineManager
{
    public class BasicProcedureStatement
    {
        public BasicProcedureStatement()
        {
            Enabled = true;
        }
        public DBTable Table { get; set; }
        [TypeConverter(typeof(IStatementListTypeConverter))]
        public SQLStatement Statement { get; set; }

        public bool Enabled { get; set; }

        public override string ToString()
        {
            return Statement != null ? Statement.ToString() : "-";
        }

        public BasicProcedureStatementDefinition GetDefinition()
        {
            var def = new BasicProcedureStatementDefinition { Enabled = Enabled, Statement = Statement.ID };

            return def;
        }

    }
}
