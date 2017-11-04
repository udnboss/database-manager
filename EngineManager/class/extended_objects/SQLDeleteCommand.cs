using System.Linq;

namespace EngineManager
{
    public class SQLDeleteCommand : SQLStatement
    {
        public override string GetSyntax()
        {
            /*
                delete  [target_schema].[target_table]
                    from [schema].[table] [tbl]
                        joins...
                    where
                        <policy>
             */

            var sql_delete = string.Format("delete from {0}", TargetTable.FullName);
            
            //joins
            sql_delete += WherePolicy == null ? "" : WherePolicy.GetQuery(false, true);

            //where key
            sql_delete += (WherePolicy == null ? "\r\nwhere" : "") + " [tbl].[" + Table.PrimaryKey.Columns.First().Name + "] = @pk_value";

            //where
            //if (WherePolicy != null)
            //    sql_insert += "\r\nAND\r\n" + WherePolicy.WhereExpression;

            return sql_delete;
        }

        internal SQLDeleteCommandDefinition GetDefinition()
        {
            var def = new SQLDeleteCommandDefinition
            {
                ID = ID,
                Name = Name,
                TargetSchema = TargetTable.Schema.Name,
                TargetTable = TargetTable.Name,
                Table = Table.Name,
                WherePolicy = WherePolicy != null ? WherePolicy.ID : null

            };

            return def;
        }
    }
}
