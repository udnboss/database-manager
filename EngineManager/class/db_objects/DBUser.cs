namespace EngineManager
{
    public class DBUser : DBObject
    {
        public DBUser() : base()
        {
            this.type = DB.DBObjectType.User;
        }
        public string FriendlyName { get; set; }
        public override string DisplayName
        {
            get { return FriendlyName; }
        }
    }
}
