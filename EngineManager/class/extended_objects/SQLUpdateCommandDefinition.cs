using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class SQLUpdateCommandDefinition
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public List<KeyValuePair<string, string>> ColumnsValues { get; set; }
        public string WherePolicy { get; set; }


        public SQLUpdateCommand GetObject(DBTable t)
        {
            var cmd = new SQLUpdateCommand
            {
                ID = ID,
                Name = Name,
                Table = t,
                WherePolicy = t.AvailablePolicies.Where(p => p.ID == WherePolicy).FirstOrDefault()                
            };

            foreach(var cv in ColumnsValues)
            {
                cmd.ColumnsValues.Add(new SQLUpdateColumnValuePair { Table = t, Column = t.Columns.Where(c => c.Name == cv.Key).FirstOrDefault(), Value = cv.Value });
            }          

            return cmd;
        }
    }
}
