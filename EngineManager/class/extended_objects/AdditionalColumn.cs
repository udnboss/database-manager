using System;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace EngineManager
{
    public class AdditionalColumn : UniqueID
    {
        [ReadOnly(true), Browsable(false)]
        public DBTable CurrentTable { get; set; }
        [ReadOnly(true)]
        public DBFKConstraint Constraint { get; set; }
        [ReadOnly(true)]
        public DBColumn Column { get; set; }

        [Category("Appearance")]
        public string Alias { 
            get 
            {
                if (!string.IsNullOrEmpty(this.CustomAlias))
                    return this.CustomAlias;

                if(this.IsFromSubTable)
                {
                    var aggr = this.Aggregation != DBAggregation.None ? this.Aggregation.ToString() : this.SortDirection == DBSort.Asc ? "first" : "last";
                    return aggr + '_' + this.Column.Parent.Name + '_' + this.Column.Name;
                }
                else
                {
                    return this.Constraint != null ? (this.Constraint.Column.Name + '_' + this.Column.Parent.Name + '_' + this.Column.Name) : this.Column.Parent.Name + '_' + this.Column.Name;
                }
                
            } 
        }
        [Category("Appearance")]
        public string CustomAlias { get; set; }

        private bool output = true;
        [Category("Appearance"), Browsable(false)]
        public bool Output { get { return output; } set { output = value; } }

        [ReadOnly(true), Browsable(false)]
        public bool IsFromSubTable { get { return this.Constraint != null ? (this.CurrentTable == this.Constraint.Parent ? false : true) : false;} }
        [ReadOnly(true), Browsable(false)]
        public bool FromThisTable { get; set; }
        
        public enum DBAggregation { None, Count, Sum, Avg, Min, Max, CountDistinct }
        [Category("Sub Table Column")]
        public DBAggregation Aggregation { get; set; }

        public enum DBSort { Asc, Desc}
        [Category("Sub Table Column")]
        public DBSort SortDirection { get; set; }

        [TypeConverter(typeof(ColumnListTypeConverter))]
        [Category("Sub Table Column")]
        public DBColumn SortColumn { get; set; }
        [Description("if true, the value will always be computed in realtime. Otherwise, it will be cached.")]
        public bool Realtime { get; set; }

        [Browsable(false)]
        [Description("use {0} as a placeholder for the column in the expression.")]
        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        public string Expression { get; set; }

        [Description("Where Expression for value returned from sub-table")]
        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        public string WhereExpression { get; set; }

        //return this col node only
        public TreeNode GetNode()
        {
            return new TreeNode((this.IsFromSubTable ? this.Constraint.Parent.Name : this.Column.Parent.Name) + '.' + this.Column.Name + " as [" + Alias + "]" ) { Tag = this, Checked = output, ImageKey = "column" };
        }

        //returns the whole tree of available related columns in the table of this column
        public TreeNode GetTree()
        {
            var source_table = (this.IsFromSubTable ? this.Constraint.Parent : this.Constraint.ReferenceColumn.Parent) as DBTable;
            var through_column = " (" + this.Constraint.Column.Name + ")";

            var table_node = new TreeNode(source_table.DisplayName + through_column) { Tag = source_table, ImageKey = "table" };

            table_node.Nodes.AddRange(source_table.Columns.Select(c => new TreeNode(c.Name)
            {
                 ImageKey = "column"
                ,Tag = new AdditionalColumn {
                    CurrentTable = this.CurrentTable,
                    Column = c, 
                    Constraint = this.Constraint
                } }).ToArray());

            //does this table contain an extended view?
            var extensions_schema = source_table.Schema.Database.Schemas.Where(x => x.Name == "views").FirstOrDefault();
            if(extensions_schema != null)
            {
                var extended_view = extensions_schema.Views.Where(x => x.Name.Equals(source_table.Schema.Name + "_" + source_table.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if(extended_view != null)
                {
                    table_node.Nodes.AddRange(extended_view.Columns.Select(c => new TreeNode(c.Name + " (extension)")
                    {
                        ImageKey = "added_column"
                        ,Tag = new AdditionalColumn
                        {
                            CurrentTable = this.CurrentTable,
                            Column = c,
                            Constraint = this.Constraint
                        }
                    }).ToArray());
                }
            }
            
            return table_node;
        }

        //returns definition that can be serialized for storing the value
        public AdditionalColumnDefinition GetDefinition()
        {
            var definition = new AdditionalColumnDefinition
            {
                Constraint = this.Constraint != null ? this.Constraint.Name : null,
                Column = this.Column.Name,
                Alias = this.Alias,
                Output = this.Output,
                CustomAlias = this.CustomAlias,
                IsFromSubTable = this.IsFromSubTable,
                Aggregation = this.Aggregation.ToString(),
                SortDirection = this.SortDirection.ToString(),
                SortColumn = this.SortColumn != null ? this.SortColumn.Name : null,
                FromThisTable = this.FromThisTable,
                Expression = this.Expression,
                Realtime = this.Realtime
            };

            return definition;
        }

        public string GetSelectSyntax()
        {
            var cmd_select = "";
            if (FromThisTable)
            {
                cmd_select = string.Format("\r\n\t\t, [{0}] = [tbl].{1}", this.Alias, this.Column.FullName);
            }
            else if (IsFromSubTable)
            {
                var parent_col = Constraint != null ? Constraint.ReferenceColumn : Column;
                var aggr = Aggregation != AdditionalColumn.DBAggregation.None ? Aggregation.ToString() : "";

                cmd_select = string.Format("\r\n\t\t, [{0}] = (select top 1 {1}([s].{2}) from {3} [s] where [s].{4} = {5} {6} {7} {8})",
                        Alias,
                        aggr,
                        Column.FullName,
                        Column.Parent.FullName,
                        //(Constraint != null ? Constraint.Owner as DBTable : CurrentTable).FullName, //sub table
                        (Constraint != null ? Constraint.Column : Column).FullName, //sub column

                        Constraint.Owner.Schema.Name == "history" ? //special cast for history
                                string.Format("cast([tbl].{0} as nvarchar(max))", parent_col.FullName)
                                : string.Format("[tbl].{0}", parent_col.FullName),

                        /* where expression if any*/
                        string.IsNullOrEmpty(WhereExpression) ? "" : "and " + WhereExpression,
                        aggr == "" ? SortColumn != null ? string.Format("order by [s].{0}", SortColumn.FullName) : "" : "",
                        aggr == "" ? SortColumn != null ? SortDirection.ToString() : "" : ""
                        );
            }
            else //select from parent tables
            {
                cmd_select = string.Format("\r\n\t\t, [{0}] = {1}.{2}", Alias, Column.Parent.FullName, Column.FullName);
               // cmd_select = string.Format("\r\n\t\t, [{0}] = {1}.{2}", Alias, Constraint.ReferenceColumn.Owner.FullName, Constraint.ReferenceColumn.FullName);
            }

            return cmd_select;
        }

        public string GetJoinSyntax(bool inner = false)
        {
            var cmd_join = "";
            if (Constraint != null)
            {
                if (IsFromSubTable)
                {
                    cmd_join = string.Format("\r\n\t\t{3} join {0} on {0}.{1} = [tbl].{2}"
                                    , Column.Parent.FullName
                                    , Constraint.Column.FullName
                                    , Constraint.ReferenceColumn.FullName
                                    , inner ? "" : "left");
                }
                else
                {
                    cmd_join = string.Format("\r\n\t\t{3} join {0} on {0}.{1} = [tbl].{2}"
                                    , Column.Parent.FullName
                                    , Constraint.ReferenceColumn.FullName
                                    , Constraint.Column.FullName
                                    , inner ? "" : "left");
                }                
            }
            return cmd_join;
        }

        public string GetOuterSelectSyntax()
        {
            var sql_outer_select = "";
            if(Output)
                sql_outer_select += "\r\n\t, [" + Alias + "] = " + "[t].[" + Alias + "]";
            return sql_outer_select;
        }

        public override string ToString()
        {
            return Alias;
        }
    }
}
