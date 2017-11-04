namespace EngineManager
{
    public class DBDatabaseTrigger : DBObject
    {
        public bool Enabled { get; set; }
        public new DB Owner { get; set; }
    }
}
