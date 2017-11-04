namespace EngineManager
{
    public class SQLDeleteCommandDefinition
    {
        public string ID { get; set; }
        public string Table { get; set; }
        public string TargetTable { get; set; }
        public string WherePolicy { get; set; }

        public string Name { get; set; }
        public string TargetSchema { get; internal set; }
    }
}
