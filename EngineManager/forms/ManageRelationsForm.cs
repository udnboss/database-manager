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
    public partial class ManageRelationsForm : Form
    {
        public ManageRelationsForm()
        {
            InitializeComponent();
        }

        private DBTable table;
        public DBTable Table
        {
            get { return this.table; }
            set { this.table = value; RefreshView(); }
        }

        private void RefreshView()
        {
            var bs = new BindingSource(table.ForeignKeys.Where(fk=> fk.IsLookup).ToList(), null);
            bs.CurrentChanged += bs_CurrentChanged;
            dataGridView1.DataSource = bs;

            var bsp = new BindingSource(table.ForeignKeys.Where(fk => fk.IsLookup == false).ToList(), null);
            bsp.CurrentChanged += bsp_CurrentChanged;
            dataGridView_parents.DataSource = bsp;
        }

        void bs_CurrentChanged(object sender, EventArgs e)
        {
            var fk = (sender as BindingSource).Current as DBFKConstraint;
            propertyGrid1.SelectedObject = (sender as BindingSource).Current;

            table.Connection.GetDataTable(string.Format("select * from [{0}].[{1}]", fk.ReferenceSchemaName, fk.ReferenceTableName));
            dataGridView2.DataSource = table.Connection.bindingSource;
        }

        void bsp_CurrentChanged(object sender, EventArgs e)
        {
            var fk = (sender as BindingSource).Current as DBFKConstraint;
            propertyGrid2.SelectedObject = (sender as BindingSource).Current;
        }

        private void toolStripButton_fix_lookup_Click(object sender, EventArgs e)
        {
            var fk = (propertyGrid1.SelectedObject as DBFKConstraint);
            var schema = fk.ReferenceSchemaName;
            var table = fk.ReferenceTableName;

            var db_schema = fk.Owner.Schema.Database.Schemas.FirstOrDefault(s => s.Name == schema);
            DBSchemaManager.Refresh(db_schema);

            var referenced_table = db_schema.Tables.FirstOrDefault(t => t.Name == table);

            //todo fix
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            new NewMultiSelection().ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            var form = new SelectTableForm();
            form.Mode = SelectTableForm.Modes.Table;
            form.DBObject = table;
            form.ShowDialog();

            form.FormClosing += form_FormClosing;
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            var ref_column = (sender as SelectTableForm).SelectedObject as DBColumn;

            Lookups.Add(new DBFKConstraint { 
                Action = DB.DBAction.Add
                , Column = ref_column
                , Connection = table.Connection
                , Schema = table.Schema
                , Name = string.Format("FK_{0}_{1}_{2}_ref_{3}_{4}_{5}", table.Schema.Name, table.Name, "column", ref_column.Schema.Name, ref_column.Parent.Name, ref_column.Name)
                , State = DBObject.DBObjectState.New
            });
        }

        public List<DBFKConstraint> Lookups = new List<DBFKConstraint>();
    }
}
