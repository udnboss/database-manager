using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace EngineManager
{
    public class AdditionalComputedColumn : UniqueID
    {
        [Category("Appearance")]
        public string Alias { get; set; }

        [Editor(typeof(SQLEditor), typeof(UITypeEditor))]
        public string Expression { get; set; }

        public string GetOuterSelectSyntax()
        {
            var sql_outer_select = "\r\n\t, [" + Alias + "] = (" + Expression + ")";
            return sql_outer_select;
        }

        public string GetOuterMostSelectSyntax()
        {
            var sql_outermost_select = "\r\n\t, [" + Alias + "] = [t].[" + Alias + "]";
            return sql_outermost_select;
        }

        public override string ToString()
        {
            return Alias;
        }

        public TreeNode GetNode()
        {
            return new TreeNode(Alias) { Tag = this };
        }

        public bool Realtime { get; set; }
    }
}
