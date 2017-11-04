using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    public class SQLInsertCommand : SQLStatement
    {
        public SQLInsertCommand() : base()
        {
            ColumnsValues = new BindingList<SQLInsertColumnValuePair>();
            ColumnsValues.ListChanged += ColumnsValues_ListChanged;
        }

        private void ColumnsValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.ItemAdded)
            {
                ColumnsValues[e.NewIndex].Table = Table;
                ColumnsValues[e.NewIndex].TargetTable = TargetTable;
            }
        }

        public BindingList<SQLInsertColumnValuePair> ColumnsValues { get; set; }

        public override string GetSyntax()
        {
            /*
                insert into [target_schema].[target_table] (col1, col2,...)
                    select 
                        col1 = [tbl].col, 
                        col2 = [tbl].col ...
                    from [schema].[table] [tbl]
                        joins...
                    where
                        <policy>
             */

            if (TargetTable == null) return "";

            var sql_insert = string.Format("insert into {0} ({1})", TargetTable.FullName, string.Join(", ", ColumnsValues.Select(cv=>cv.TargetColumn.FullName).ToList()));
            sql_insert += "\r\nselect" +  string.Join(",\r\n\t", ColumnsValues.Select(cv => cv.ToString()).ToList());
            //sql_insert += "\r\nfrom " + Table.FullName + " [tbl]";

            //joins
            sql_insert += WherePolicy == null ? "" : WherePolicy.GetQuery(false, true);

            //where key
            sql_insert += (WherePolicy == null ? "\r\nwhere" : "") + " [tbl].[" + Table.PrimaryKey.Columns.First().Name + "] = @pk_value";

            //where
            //if (WherePolicy != null)
            //    sql_insert += "\r\nAND\r\n" + WherePolicy.WhereExpression;

            return sql_insert;
        }

        internal SQLInsertCommandDefinition GetDefinition()
        {
            var def = new SQLInsertCommandDefinition
            {
                ID = ID,
                Name = Name,
                TargetSchema = TargetTable.Schema.Name,
                TargetTable = TargetTable.Name,
                WherePolicy = WherePolicy != null ? WherePolicy.ID : null,
                ColumnsValues = ColumnsValues.Select(cv=> cv.GetDefinition()).ToList()
            };

            return def;
        }
    }
}
