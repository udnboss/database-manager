using System.Linq;
using System.Windows.Forms;
using System.ComponentModel;

namespace EngineManager
{
    public class DBProject : IGenerateTreeNode
    {
        public DBProject()
        {
            this.Connections = new BindingList<DBConnection>();
            this.Connections.AddingNew += (sender, e) => {
            	(e.NewObject as DBConnection).Project = this;
            };
            
            this.Commands = new BindingList<DBCommand>();
        }

        public BindingList<DBCommand> Commands { get; set;}
        public BindingList<DBConnection> Connections { get; set;}
        public string Name { get; set; }
        public TreeNode GetTree()
        {
            var root = new TreeNode(this.Name) { Tag = this };
            root.Nodes.AddRange(Connections.Select(x => x.GetTree()).ToList().ToArray());
            return root;
        }
    }
}
