using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EngineManager
{
    public partial class DBCommandPreview : Form
    {
        public DBCommandPreview()
        {
            InitializeComponent();

            // Load from XML
            syntaxEditor1.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);
        }

        protected DBCommand cmd;
        public DBCommand Command { get { return this.cmd; } set { this.cmd = value; syntaxEditor1.Text = value.Sql; } }

        private void toolStripButton_execute_Click(object sender, EventArgs e)
        {
            Command.Sql = syntaxEditor1.Text;
            DBProjectManager.Execute(Command);
            this.Close();
        }

        private void toolStripButton_reset_Click(object sender, EventArgs e)
        {
            syntaxEditor1.Text = cmd.Sql;
        }
    }
}
