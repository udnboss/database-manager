using System.ComponentModel;

namespace EngineManager
{
    public class TriggerConstraint : UniqueID
    {
        public TriggerConstraint() : base()
        {
            
        }

        [Description("message to display when the expression fails")]
        public string FailMessage { get; set; }
        public bool Enabled { get; set; }
        public string Expression { get; set; }
        public DBTable Table { get; set; }
    }
}
