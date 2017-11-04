using System.ComponentModel;

namespace EngineManager
{
    public class DBRole : DBObject
    {
        public DBRole() : base()
        {
            this.type = DB.DBObjectType.Role;
            this.Members = new BindingList<DBUser>();
        }
        public BindingList<DBUser> Members { get; set; }
        public override string DisplayName 
        {
            get { return this.Name; }        
        }
    }
}
