using System.ComponentModel;

namespace EngineManager
{
    public class SQLUpdateColumnValuePair : UniqueID
    {
        public DBTable Table { get; set; }
        [TypeConverter(typeof(ColumnListTypeConverter))]
        public DBColumn Column { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return (Column != null ? Column.FullName : "?") + " = " + Value;
        }
    }
}
