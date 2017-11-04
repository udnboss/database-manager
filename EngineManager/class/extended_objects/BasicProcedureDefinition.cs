using System.Collections.Generic;

namespace EngineManager
{
    public class BasicProcedureDefinition
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Table { get; set; }

        public List<BasicProcedureStatementDefinition> Statements { get; set; }

        public BasicProcedure GetObject(DBTable t)
        {
            var obj = new BasicProcedure { ID = ID, Name = Name, Table = t };

            foreach (var s in Statements)
                obj.Statements.Add(s.GetObject(t));

            return obj;
        }
    }
}
