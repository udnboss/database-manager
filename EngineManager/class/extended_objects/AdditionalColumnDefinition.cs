using System;
using System.Data;
using System.Linq;

namespace EngineManager
{
    public class AdditionalColumnDefinition
    {
        public string CurrentTable { get; set; }
        public string Constraint { get; set; }
        public string Column { get; set; }
        public string Alias { get; set; }
        public string CustomAlias { get; set; }
        private bool output = true;//default to true
        public bool Output { get { return output; } set { output = value; } }
        public bool IsFromSubTable { get; set; }
        public string Aggregation { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
        public string Expression { get; set; }
        public string WhereExpression { get; set; }
        public bool FromThisTable { get; set; }
        public bool Realtime { get; set; }

        public AdditionalColumn GetObject(DBTable t)
        {
            AdditionalColumn result;

            if (FromThisTable)
            {
                result = new AdditionalColumn
                {
                    Constraint = null,
                    Column = t.Columns.Where(c => c.Name == Column).FirstOrDefault(),
                    CurrentTable = t,
                    CustomAlias = CustomAlias,
                    Output = Output,
                    SortColumn = null,
                    SortDirection = AdditionalColumn.DBSort.Asc,
                    Aggregation = AdditionalColumn.DBAggregation.None,
                    Expression = Expression,
                    FromThisTable = true
                };
            }
            else if(IsFromSubTable) //from sub table
            {
                var fk = t.DBObjects.Where(x => x is DBFKConstraint).Cast<DBFKConstraint>().Where(c => c.Name == Constraint).FirstOrDefault();

                result = new AdditionalColumn
                {
                    Constraint = fk,
                    Column = (fk.Parent as DBTable).CombinedColumns.Where(c => c.Name == Column).FirstOrDefault(),
                    CurrentTable = t,
                    CustomAlias = CustomAlias,
                    Output = Output,
                    SortColumn = (fk.Parent as DBTable).Columns.Where(c => c.Name == SortColumn).FirstOrDefault(),
                    SortDirection = (AdditionalColumn.DBSort)Enum.Parse(typeof(AdditionalColumn.DBSort), SortDirection),
                    Aggregation = (AdditionalColumn.DBAggregation)Enum.Parse(typeof(AdditionalColumn.DBAggregation), Aggregation),
                    Expression = Expression
                };
            }
            else //from parent
            {
                var fk = t.DBObjects.Where(x => x is DBFKConstraint).Cast<DBFKConstraint>().Where(c => c.Name == Constraint).FirstOrDefault();
                
                //if((fk.ReferenceColumn.Owner as DBTable).CombinedColumns.Count == 0)
                //    fk.ReferenceColumn.Owner.Refresh(); //todo find better performance solution
                if (fk == null)
                    return null;

                result = new AdditionalColumn
                {
                    Constraint = fk,
                    Column = (fk.ReferenceColumn.Parent as DBTable).CombinedColumns.Where(c => c.Name == Column).FirstOrDefault(),
                    CurrentTable = t,
                    CustomAlias = CustomAlias,
                    Output = Output,
                    SortColumn = null,
                    SortDirection = AdditionalColumn.DBSort.Asc,
                    Aggregation = AdditionalColumn.DBAggregation.None,
                    Expression = Expression
                };

            }

            return result;
        }
    }
}
