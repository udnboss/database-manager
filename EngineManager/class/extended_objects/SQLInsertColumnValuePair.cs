using System.ComponentModel;

namespace EngineManager
{
    public class SQLInsertColumnValuePair : UniqueID
    {
        [ReadOnly(true), Browsable(false)]
        public DBTable Table { get; set; }
        [ReadOnly(true), Browsable(false)]
        public DBTable TargetTable { get; set; }

        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn TargetColumn { get; set; }
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn SourceColumn { get; set; }
        public string ConstantValue { get; set; }

        public override string ToString()
        {
            return (TargetColumn != null ? TargetColumn.FullName : "?") + " = " + (SourceColumn != null && ConstantValue == null ? SourceColumn.FullName : ConstantValue);
        }

        public SQLInsertColumnValuePairDefinition GetDefinition()
        {
            return new SQLInsertColumnValuePairDefinition
            {
                ID = ID,
                Table = Table.Name,
                SourceColumn = SourceColumn.Name,
                ConstantValue = ConstantValue,
                TargetSchema = TargetTable.Schema.Name,
                TargetTable = TargetTable.Name,
                TargetColumn = TargetColumn.Name
            };
        }
    }
}
