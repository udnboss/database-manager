using System.Data;
using System.Linq;

namespace EngineManager
{
    public class BasicProcedureStatementDefinition
    {
        public bool Enabled { get; set; }
        public string Statement { get; set; }

        public BasicProcedureStatement GetObject(DBTable t)
        {
            var obj = new BasicProcedureStatement { Enabled = Enabled, Statement = t.UpdateCommands.Where(c => c.ID == Statement).FirstOrDefault(), Table = t };
            return obj;
        }
    }
}
