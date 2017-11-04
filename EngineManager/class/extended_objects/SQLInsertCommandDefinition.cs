using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class SQLInsertCommandDefinition
    {
        public string ID { get; set; }
        public string Table { get; set; }
        public string TargetSchema { get; set; }
        public string TargetTable { get; set; }
        public List<SQLInsertColumnValuePairDefinition> ColumnsValues { get; set; }
        public string WherePolicy { get; set; }

        public string Name { get; set; }

        public SQLInsertCommand GetObject(DBTable t)
        {
            var cmd = new SQLInsertCommand
            {
                ID = ID,
                Name = Name,
                Table = t,
                WherePolicy = t.AvailablePolicies.Where(p => p.ID == WherePolicy).FirstOrDefault()
                //TargetTable = TargetTable, //todo
                
            };

            foreach (var cv in ColumnsValues)
            {
                cmd.ColumnsValues.Add(new SQLInsertColumnValuePair
                {
                    Table = t,
                    SourceColumn = t.Columns.Where(c => c.Name == cv.SourceColumn).FirstOrDefault(),
                    ConstantValue = cv.ConstantValue
                    //todo
                });
            }

            return cmd;
        }
    }
}
