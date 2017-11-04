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
    public partial class SQLEditorForm : Form
    {
        public SQLEditorForm()
        {
            InitializeComponent();

            // Load from XML
            syntaxEditor1.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);
        }

        public string SQL
        {
            get
            {
                return syntaxEditor1.Text;
            }
            set
            {
                syntaxEditor1.Text = value;
            }
        }
    }
}
