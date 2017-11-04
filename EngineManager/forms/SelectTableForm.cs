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
    public partial class SelectTableForm : Form
    {
        public SelectTableForm()
        {
            InitializeComponent();
        }
        private DBObject db_object;

        public enum Modes { Table, Column } 
        public DBObject DBObject
        {
            get { return db_object; }
            set { db_object = value; RefreshView(); }
        }
        public Modes Mode { get; set; }

        private void RefreshView()
        {
            treeView1.Nodes.AddRange(db_object.Connection.GetDataTable(Util.GetSql("get_schemas")).AsEnumerable().Select(x =>
                    new TreeNode
                    {
                        Text = x["name"].ToString()
                        , Name = x["name"].ToString()
                        , Tag = new DBSchema
                            {
                                Name = x["name"].ToString(),
                                Connection = db_object.Connection
                            }
                    }).ToArray());
        }

        public DBObject SelectedObject { get { return treeView1.SelectedNode.Tag != null ? treeView1.SelectedNode.Tag as DBObject : null; } }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            
            if (e.Node.Nodes.Count == 0 && e.Node.Tag != null)
            {
                var obj = e.Node.Tag;
                var db_obj = obj as DBObject;
                var sql = "";
                if (obj is DBSchema)
                    sql = string.Format(Util.GetSql("get_schema_tables_and_views"), db_obj.Name);

                if(obj is DBContainsColumns)
                	sql = string.Format(Util.GetSql("get_object_columns"), (db_obj as DBSchemaObject).Schema, db_obj.Name);

                if (obj is DBColumn)
                    return;

                e.Node.Nodes.AddRange(db_obj.Connection.GetDataTable(sql).AsEnumerable().Select(x =>
                    new TreeNode
                    {
                        Text = x["name"].ToString()
                        , Name = x["name"].ToString()
                        , Tag = obj is DBSchema ? new DBTable
                        {
                        	Schema = (db_obj as DBSchemaObject).Schema,
                            Name = x["name"].ToString(),
                            Connection = db_object.Connection
                        } as DBObject
                        :
                        new DBColumn
                        {
                            Parent = db_obj,
                            Schema = (db_obj as DBSchemaObject).Schema,
                            Name = x["name"].ToString(),
                            Connection = db_object.Connection
                        } as DBObject
                    }).ToArray());
            }
        }
    }
}
