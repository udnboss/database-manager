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
    public partial class ManageCheckConstraints : Form
    {
        public ManageCheckConstraints()
        {
            InitializeComponent();
        }

        private DBTable db_table;
        public DBTable DBTable
        {
            get
            {
                return db_table;
            }
            set
            {
                db_table = value;
                RefreshView();
            }
        }

        private void RefreshView()
        {
            listBox1.DataSource = new BindingSource(DBTable.CheckConstraints, null);
            listBox2.DataSource = new BindingSource(DBTable.TriggerConstraints, null);
            listBox1.DisplayMember = "Name";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBox1.SelectedItem;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = listBox2.SelectedItem;
        }

        private void btn_add_cc_Click(object sender, EventArgs e)
        {
            DBTable.CheckConstraints.Add(new DBCheckConstraint { Parent = DBTable });
        }

        private void btn_add_tc_Click(object sender, EventArgs e)
        {
            DBTable.TriggerConstraints.Add(new TriggerConstraint { Table = DBTable });
        }
    }
}
