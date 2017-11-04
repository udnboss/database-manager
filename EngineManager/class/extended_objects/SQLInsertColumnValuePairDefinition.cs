namespace EngineManager
{
    public class SQLInsertColumnValuePairDefinition
    {
        public string ID { get; set; }
        public string Table { get; set; }
        public string TargetSchema { get; set; }
        public string TargetTable { get; set; }
        public string TargetColumn { get; set; }
        public string SourceColumn { get; set; }
        public string ConstantValue { get; set; }
    }
}
