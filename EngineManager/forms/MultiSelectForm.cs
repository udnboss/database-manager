using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace EngineManager
{
    public partial class MultiSelectForm : Form
    {
        public MultiSelectForm()
        {
            InitializeComponent();

            
        }

        private DBObject db_object;
        public DBObject DBObject
        {
            get { return this.DBObject; }
            set
            {
                this.db_object = value;

            }
        }
        public void populate_list()
        {
            checkedListBox1.Items.Clear();

            if(this.db_object is DBTable)
            {
                checkedListBox1.Items.AddRange((this.db_object as DBTable).CombinedColumns.Select(c => c.Name).ToList().ToArray());

                for (int i = 0; i < this.Value.Count; i++)
                {
                    var ind = checkedListBox1.Items.IndexOf(this.Value[i].Trim());
                    if(ind > -1)
                        checkedListBox1.SetItemChecked(ind, true);
                }
            }

            if (this.db_object is DBColumn)
            {
                checkedListBox1.Items.AddRange((this.db_object.Parent as DBTable).CombinedColumns.Select(c => c.Name).ToList().ToArray());

                for (int i = 0; i < this.Value.Count; i++)
                    checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(this.Value[i].Trim()), true);
            }
        }

        private List<string> val = new List<string>();
        public List<string> Value { get { return val; } set { val = value; } }

        private void button1_Click(object sender, EventArgs e)
        {
            this.val.Clear();

            foreach (var o in checkedListBox1.CheckedItems)
                this.Value.Add(o.ToString());

            if (db_object is DBTable)
            {
                (this.db_object as DBTable).LookupTooltipColumns = this.Value;
            }
            else if(db_object is DBColumn)
            {
                (this.db_object as DBColumn).SequenceIdentifiers = this.Value;
            }
        }
    }
}
