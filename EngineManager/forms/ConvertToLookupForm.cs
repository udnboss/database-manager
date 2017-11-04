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
    public partial class ConvertToLookupForm : Form
    {
        public ConvertToLookupForm()
        {
            InitializeComponent();
        }

        private DBColumn column;

        public DBColumn Column
        {
            get { return column; }
            set 
            { 
                column = value; 
                lookup.Column = column; 
                lookup.LookupTableName = column.Parent.Name + "_" + column.Name.ToLower() + "s"; 
                propertyGrid1.SelectedObject = lookup;
                lbl_target_col.Text = column.Name;
                RefreshView(); 
            }
        }

        private LookupConversion lookup = new LookupConversion() { Expression = "{0}", NameLength = 50 };

        private void RefreshView()
        {
            var sql = lookup.GetPreview();
            dataGridView_preview.DataSource = column.Connection.GetDataTable(sql);
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RefreshView();
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            lookup.CreateLookupTable();
        }
    }

    public class LookupConversion
    {
        public DBColumn Column { get; set; }
        //public bool UseUniqueIdentifier { get; set; }

        public string LookupTableName { get; set; }

        public string Expression { get; set; }

        public int NameLength { get; set; }

        public string GetPreview()
        {
            var sql = "";

            var current_table = Column.Parent as DBTable;
            var expression = Expression == null ? Column.FullName : string.Format(Expression, Column.FullName);

            sql = string.Format("select distinct [id] = cast({0} as nvarchar({1})), [name] = cast({2} as nvarchar({3})) from {4}", expression, Column.Length, Column.FullName, NameLength, current_table.FullName);

            return sql;
        }

        public void CreateLookupTable()
        {
            var owner = (Column.Parent as DBTable);
            var sql = string.Format("if object_id('[{0}].[{1}]') is null select * into [{0}].[{1}] from ({2}) t ", Column.Owner.Schema.Name, LookupTableName, GetPreview());
            
            var cmd_create_table = new DBCommand { Owner = owner, Sql = sql, Description = "Create Lookup Table" };
            owner.Connection.Project.Commands.Add(cmd_create_table);
            DBProjectManager.Execute(cmd_create_table);

            //refresh so now the schema has the table..
            DBSchemaManager.Refresh(Column.Schema);

            var lookup_table = Column.Schema.Tables.FirstOrDefault(t => t.Name == LookupTableName);
            DBTableManager.Refresh(lookup_table);
            lookup_table.Action = DB.DBAction.Alter;

            var pk_col = lookup_table.Columns.FirstOrDefault(x => x.Name == "id");
            pk_col.Action = DB.DBAction.Alter;
            pk_col.Nullable = false;
            var cmd_not_null = pk_col.Connection.Project.Commands.FirstOrDefault(c => c.Owner == pk_col);

            if (cmd_not_null != null)
                DBProjectManager.Execute(cmd_not_null);

            //create the PK
            lookup_table.PrimaryKey = new DBPKConstraint
            {
                Action = DB.DBAction.Add,
                Parent = lookup_table,
                Schema = lookup_table.Schema,
                Connection = lookup_table.Connection,
                Name = "PK_" + LookupTableName
            };

            lookup_table.PrimaryKey.Columns.Add(pk_col);


            var cmd = new DBCommand { Owner = lookup_table, Sql = lookup_table.PrimaryKey.GetSQL(), Description = "Create PK" };
            lookup_table.Connection.Project.Commands.Add(cmd);
            DBProjectManager.Execute(cmd);

            //table.Refresh();
            
            //reference by FK
            var fk = new DBFKConstraint
            {
                Name = "FK_" + owner.Schema.Name + "_" + owner.Name + "_" + Column.Name + "_ref_" + lookup_table.Schema.Name + "_" + lookup_table.Name + "_id",
                Action = DB.DBAction.Add,
                Parent = Column.Parent,
                Schema = Column.Owner.Schema,
                Column = Column,
                Connection = Column.Connection,
                OnUpdate = DBFKConstraint.CascadeModes.Cascade,
                ReferenceColumn = pk_col
            };

            
            owner.ForeignKeys.Add(fk);

            var cmd_fk = new DBCommand { Owner = owner, Sql = fk.GetSQL(), Description = "Create FK" };
            lookup_table.Connection.Project.Commands.Add(cmd_fk);
            DBProjectManager.Execute(cmd_fk);

        }

    }
}
