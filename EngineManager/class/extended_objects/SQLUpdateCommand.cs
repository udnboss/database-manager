using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace EngineManager
{
    public class SQLUpdateCommand : SQLStatement
    {
        public SQLUpdateCommand() :  base()
        {
            ColumnsValues = new BindingList<SQLUpdateColumnValuePair>();
            ColumnsValues.ListChanged += ColumnsValues_ListChanged;
        }

        private void ColumnsValues_ListChanged(object sender, ListChangedEventArgs e)
        {
            if(e.ListChangedType == ListChangedType.ItemAdded)
            {
                ColumnsValues[e.NewIndex].Table = Table;
            }
        }

        public BindingList<SQLUpdateColumnValuePair> ColumnsValues { get; set; }

        public override string GetSyntax()
        {
            /*
                update [tbl]
                    set 
                    col = val, 
                    col = val ...
                from schema.table [tbl]
                    joins...
                where
                    <policy>
             */

            var sql_update = "update [tbl]\r\n\tset\r\n\t";
            sql_update += string.Join(",\r\n\t", ColumnsValues.Select(c => c.Column.FullName + " = " + c.Value));
            sql_update += "\r\nfrom " + Table.FullName + " [tbl]";

            //joins
            var joined_fks = new List<string>();
            foreach (var related_col in Table.AdditionalRelatedColumns)
            {
                var col_owner = related_col.Column.Parent.FullName;
                var fk_key = related_col.Constraint != null ? related_col.Constraint.FullName + "-" + col_owner : col_owner;

                if (!joined_fks.Contains(fk_key))
                {
                    var cmd_join = related_col.GetJoinSyntax();
                    sql_update += cmd_join;
                    joined_fks.Add(fk_key);
                }
                else //reuse the same join if used before
                {

                }
            }

            //where key
            sql_update += "\r\nwhere [tbl].[" + Table.PrimaryKey.Columns.First().Name + "] = @pk_value";

            //where
            if (WherePolicy!=null)
                sql_update += "\r\nAND\r\n" + WherePolicy.WhereExpression;

            return sql_update;
        }

        internal SQLUpdateCommandDefinition GetDefinition()
        {
            var def = new SQLUpdateCommandDefinition
            {
                ID = ID,
                Name = Name,
                WherePolicy = WherePolicy != null ? WherePolicy.ID : null,
                ColumnsValues = ColumnsValues.Select(cv => new KeyValuePair<string, string>(cv.Column.Name, cv.Value)).ToList()
            };

            return def;
        }
    }
}
