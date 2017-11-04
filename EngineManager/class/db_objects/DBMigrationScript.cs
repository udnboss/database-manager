using System.ComponentModel;
using System.Drawing.Design;

namespace EngineManager
{
    public class DBMigrationScript
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        public string SQLScript { get; set; }

        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        public string SystemCommand { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}
