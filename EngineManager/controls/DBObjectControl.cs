using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Glee.Drawing;
using System.Collections;

namespace EngineManager
{
    public partial class DBObjectControl : UserControl
    {
        public DBObjectControl()
        {
            InitializeComponent();

            // Load from XML
            syntaxEditor1.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);
            syntaxEditor2.Document.LoadLanguageFromXml(Application.StartupPath + "\\xml\\ActiproSoftware.SQL.xml", 0);

            
        }

        
        public DBObject DBObject { get; set; }

        public void RefreshDisplay()
        {
            var obj = this.DBObject;

            //impersonation
            toolStripComboBox_exec_grid_as_user.Items.Clear();
            toolStripComboBox_exec_grid_as_user.Items.Add(DBObject.Connection.User);
            toolStripComboBox_exec_grid_as_user.SelectedItem = DBObject.Connection.User;
            toolStripComboBox_exec_grid_as_user.Items.AddRange(obj.Connection.GetDataTable(Util.GetSql("get_impersonation")).AsEnumerable().Select(x =>
                    x["impersonated"].ToString() 
                    ).ToList().ToArray());
            
            if(!(obj is IContainsColumns))
            {
                if (tabControl1.TabPages.Contains(tabPage_design))
                    tabControl1.TabPages.Remove(tabPage_design);    
            }

            var sql = "";
            var sql_select = "";
			
            var db_obj = (obj as DBSchemaObject);
			if (db_obj!= null)
			{
				
				sql = db_obj.GetSQL();

                

				if (db_obj.Schema != null && db_obj.State != DBObject.DBObjectState.New) {
					//properties grid
					dataGridView_properties.DataSource = new BindingSource(db_obj.Properties, null);
				}
               
				//columns
				if (obj is IContainsColumns) {
					var tbl = obj as IContainsColumns;
                    
					var cols = string.Join("\r\n\t,", tbl.Columns.Select(x => x.FullName).ToList());
					sql_select = string.Format("if object_id('{1}') is not null select top 100 \r\n\t{0} \r\nfrom \r\n\t{1}", cols, db_obj.FullName);
					this.FillGrid(sql_select);
                    
					//current table as default source
					toolStripComboBox_source.Items.Clear();
					toolStripComboBox_source.Items.Add(obj);
					toolStripComboBox_source.SelectedItem = obj;


					//bind the columns to the grid    
					BindingSource cols_source = new BindingSource(tbl is DBTable ? (tbl as DBTable).CombinedColumns : tbl.Columns, null);

					dataGridView_design.AutoGenerateColumns = false;
					dataGridView_design.Columns.Clear();
					dataGridView_design.Columns.AddRange(new DataGridViewColumn[] { 
						new DataGridViewTextBoxColumn() {
							Name = "Owner",
							HeaderText = "Owner",
							DataPropertyName = "OwnerName",
							ReadOnly = true
						},
						new DataGridViewTextBoxColumn() {
							Name = "DisplayOrder",
							HeaderText = "DisplayOrder",
							DataPropertyName = "DisplayOrder"
						},
						new DataGridViewTextBoxColumn() {
							Name = "Name",
							HeaderText = "Name",
							DataPropertyName = "Name"
						},
						new DataGridViewTextBoxColumn() {
							Name = "Title",
							HeaderText = "Title",
							DataPropertyName = "Title"
						},
						new DataGridViewTextBoxColumn() {
							Name = "DataType",
							HeaderText = "DataType",
							DataPropertyName = "DataType",
							ReadOnly = true
						},
						new DataGridViewTextBoxColumn() {
							Name = "Length",
							HeaderText = "Length",
							DataPropertyName = "Length"
						},
						new DataGridViewTextBoxColumn() {
							Name = "CustomDataType",
							HeaderText = "CustomDataType",
							DataPropertyName = "CustomDataType",
							ReadOnly = true
						},
						new DataGridViewCheckBoxColumn() {
							Name = "Nullable",
							HeaderText = "Nullable",
							DataPropertyName = "Nullable"
						},
						new DataGridViewTextBoxColumn() {
							Name = "Default",
							HeaderText = "Default",
							DataPropertyName = "Default"
						}
					});

					dataGridView_design.DataSource = cols_source;
                                     
					//sources                    
					if (db_obj is DBTable) {
						toolStripComboBox_source.Items.AddRange((db_obj as DBTable).Extensions.ToArray());

					}
                    
				} else if (obj is DBColumn) {
					var db_column = (obj as DBColumn);
					sql_select = string.Format("select top 100 {0} from {1}", db_column.FullName, db_column.Parent.FullName);
					this.FillGrid(sql_select);
				}


			}
            
            syntaxEditor1.Text = string.IsNullOrEmpty(sql_select) ? sql : sql_select;


            syntaxEditor2.Text = sql;

            //glee
            //create a graph object
            Microsoft.Glee.Drawing.Graph graph = new Microsoft.Glee.Drawing.Graph("graph") { Directed = true, BuildNodeHierarchy = true, Cluster = true };
            graph.GraphAttr.AspectRatio = 2;
            graph.GraphAttr.LayerDirection = LayerDirection.TB;
            graph.GraphAttr.OptimizeLabelPositions = true;
            graph.GraphAttr.Orientation = Microsoft.Glee.Drawing.Orientation.Landscape;
           // graph.GraphAttr.LayerSep = 1;
            //graph.GraphAttr.NodeSep = 1;

            DBObject.DrawGraph(graph, null);

            //reshape
            foreach (DictionaryEntry node in graph.NodeMap)
            {
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.Shape = Microsoft.Glee.Drawing.Shape.Box;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.Fontsize = 8;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.LabelMargin = 10;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.FontName = "Lucida Console";
            }
            

            gViewer1.Graph = graph;

        }

        protected void FillGrid(string sql)
        {
            var obj = this.DBObject;


                //var sql = string.Format("select top 100 * from {0}", (obj as DBObject).FullName);
                var binding_source = obj.Connection.GetBoundSource(sql);
                dataGridView1.DataSource = binding_source;
                bindingNavigator1.BindingSource = binding_source;
                //Resize the DataGridView columns to fit the newly loaded content.
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        }

        public void Execute(string execute_as = null)
        {
            listBox1.Items.Clear();
            this.DBObject.Connection.sql_messages.Clear();

            var syntax = "";

            if (syntaxEditor1.SelectedView.SelectedText.Length > 0)
            {
                syntax = syntaxEditor1.SelectedView.SelectedText;
                //SetStatusText("Executed Selection. Done!");
            }
            else
            {
                syntax = syntaxEditor1.Text;
                //SetStatusText("Executed All. Done!");
            }

            if(!string.IsNullOrEmpty(execute_as) && execute_as != DBObject.Connection.User)
            {
                syntax = string.Format("execute as user = '{0}'\n{1}\nrevert;", execute_as, syntax);
            }

            var binding_source = ExecuteCommand(syntax);

            listBox1.Items.Add("Done.");
            listBox1.Items.AddRange(this.DBObject.Connection.sql_messages.ToArray());

            dataGridView1.DataSource = binding_source;  //use this so it is bound for CRUD operations.  
                                                                                //Resize the DataGridView columns to fit the newly loaded content.
            bindingNavigator1.BindingSource = binding_source;
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        public void ExecuteDefinition(string execute_as = null)
        {
            listBox1.Items.Clear();
            this.DBObject.Connection.sql_messages.Clear();

            var syntax = "";

            if (syntaxEditor2.SelectedView.SelectedText.Length > 0)
            {
                syntax = syntaxEditor2.SelectedView.SelectedText;
                //SetStatusText("Executed Selection. Done!");
            }
            else
            {
                syntax = syntaxEditor2.Text;
                //SetStatusText("Executed All. Done!");
            }

            if (!string.IsNullOrEmpty(execute_as) && execute_as != DBObject.Connection.User)
            {
                syntax = string.Format("execute as user = '{0}'\n{1}\nrevert;", execute_as, syntax);
            }

            var binding_source = ExecuteCommand(syntax);

            listBox1.Items.Add("Done.");
            listBox1.Items.AddRange(this.DBObject.Connection.sql_messages.ToArray());

            dataGridView1.DataSource = binding_source; // this.DBObject.Connection.bindingSource;  //use this so it is bound for CRUD operations.  
                                                       //Resize the DataGridView columns to fit the newly loaded content.
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private BindingSource ExecuteCommand(string cmd)
        {
            return this.DBObject.Connection.GetBoundSource(cmd);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Execute();            
        }

        private void mnu_messages_clear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void mnu_messages_copy_Click(object sender, EventArgs e)
        {
           Clipboard.SetText(listBox1.SelectedItem.ToString());
        }

        private void copyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            var all = string.Join("\r\n", this.DBObject.Connection.sql_messages);
            Clipboard.SetText(all);
        }

        private void toolStripButton_execute_as_Click(object sender, EventArgs e)
        {
            string user = null;
            if (toolStripComboBox_execute_as.SelectedItem != null && toolStripComboBox_execute_as.SelectedItem.ToString() != DBObject.Connection.User)
                user = toolStripComboBox_execute_as.SelectedItem.ToString();
            Execute(user);
        }

        private void toolStripComboBox_source_SelectedIndexChanged(object sender, EventArgs e)
        {
            return;

            if (this.DBObject is DBTable || this.DBObject is DBView)
            {
                var sql_select = "select top 100 * from " + this.DBObject.FullName;

                if (toolStripComboBox_source.SelectedItem != null)
                {
                    sql_select = string.Format("select top 100 * from {0}", (toolStripComboBox_source.SelectedItem as DBObject).FullName);
                }

                if (toolStripComboBox_execute_as.SelectedItem != null && toolStripComboBox_execute_as.SelectedItem.ToString() != DBObject.Connection.User) //no self impersonation
                {
                    
                    sql_select = "execute as user = '" + toolStripComboBox_execute_as.SelectedItem.ToString() + "'\r\n\t" + sql_select + "\r\n\trevert;";
                }

                syntaxEditor1.Text = sql_select;
                FillGrid(sql_select);
            }            
        }

        

        private void gViewer1_SelectionChanged(object sender, EventArgs e)
        {
            if (gViewer1.SelectedObject != null)
            {
                if (gViewer1.SelectedObject is Edge)
                {
                   // MessageBox.Show((gViewer1.SelectedObject as Edge).Source);
                }
                else if (gViewer1.SelectedObject is Node)
                {
                    var n = gViewer1.SelectedObject as Node;
                    n.Attr.Fillcolor = Microsoft.Glee.Drawing.Color.Red;
                    n.Attr.Color = Microsoft.Glee.Drawing.Color.Red;
                    //n.Attr.Label = "xxxxxxxxxxxxxx";
                    //gViewer1.ShowBBox(n.BBox);
                    gViewer1.Invalidate();
                    //gViewer1.
                  //  MessageBox.Show((gViewer1.SelectedObject as Node).Attr.Label);
                }
            }
        }




        private void comboBox_visualize_SelectedValueChanged(object sender, EventArgs e)
        {
            if(comboBox_visualize.SelectedItem.ToString() == "Relations")
            {
                Graph g = new Graph("relations");
                DBObject.DrawRelationalGraph(g, null);
                gViewer1.Graph = g;
            }

            //reshape
            foreach (DictionaryEntry node in gViewer1.Graph.NodeMap)
            {
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.Shape = Microsoft.Glee.Drawing.Shape.Box;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.Fontsize = 8;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.LabelMargin = 10;
                ((Microsoft.Glee.Drawing.Node)node.Value).Attr.FontName = "Lucida Console";
            }
        }

        
        public event EventHandler ActiveObjectChanged;
        private void dataGridView_design_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView_design_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView_design_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            //raise event when a row is entered for current column to show up in propertygrid

            if (this.ActiveObjectChanged != null)
            {
                if(this.DBObject is DBTable)
                    this.ActiveObjectChanged((DBObject as DBTable).CombinedColumns[e.RowIndex], new EventArgs());
                else
                    this.ActiveObjectChanged((DBObject as IContainsColumns).Columns[e.RowIndex], new EventArgs());
            }

            var cols_source = dataGridView_design.DataSource as BindingSource;

            //update readonly cells for datatype for views
            var r = dataGridView_design.Rows[e.RowIndex];
            var read_only = !((cols_source[e.RowIndex] as DBColumn).Parent is DBTable);
            r.Cells[4].ReadOnly = read_only; //datatype
            r.Cells[5].ReadOnly = read_only; //length
            r.Cells[6].ReadOnly = read_only; //nullable
            
        }

        private void dataGridView_design_Click(object sender, EventArgs e)
        {
            //top left click will cause deselection and change property grid into owner object. eg. table
            MouseEventArgs args = (MouseEventArgs)e;
            DataGridView dgv = (DataGridView)sender;
            DataGridView.HitTestInfo hit = dgv.HitTest(args.X, args.Y);
            if (hit.Type == DataGridViewHitTestType.TopLeftHeader)
            {
                //select none
                dgv.ClearSelection();
                if (this.ActiveObjectChanged != null)
                {
                    this.ActiveObjectChanged(this.DBObject, new EventArgs());
                }
            }
        }

        private void dataGridView_design_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView_design.ClearSelection();
        }

        private void checkedListBox_roles_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox_roles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }


        private void toolStripButton_exec_grid_as_Click(object sender, EventArgs e)
        {
            if (this.DBObject is DBTable || this.DBObject is DBView)
            {
                var sql_select = "select top 100 * from " + this.DBObject.FullName;

                if (toolStripComboBox_source.SelectedItem != null)
                {
                    sql_select = string.Format("select top 100 * from {0}", (toolStripComboBox_source.SelectedItem as DBObject).FullName);
                }

                if (toolStripComboBox_exec_grid_as_user.Text != null && toolStripComboBox_exec_grid_as_user.Text != DBObject.Connection.User)
                {
                    sql_select = "execute as user = '" + toolStripComboBox_exec_grid_as_user.Text + "'\r\n\t" + sql_select + "\r\n\trevert;";
                }

                syntaxEditor1.Text = sql_select;
                FillGrid(sql_select);
            }  
        }

        private void toolStripComboBox_exec_grid_as_user_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DBObject is DBTable || this.DBObject is DBView)
            {
                var sql_select = "select top 100 * from " + this.DBObject.FullName;

                if (toolStripComboBox_source.SelectedItem != null)
                {
                    sql_select = string.Format("select top 100 * from {0}", (toolStripComboBox_source.SelectedItem as DBObject).FullName);
                }

                if (toolStripComboBox_exec_grid_as_user.SelectedItem != null && toolStripComboBox_exec_grid_as_user.SelectedItem.ToString() != DBObject.Connection.User)
                {
                    sql_select = "execute as user = '" + toolStripComboBox_exec_grid_as_user.Text + "'\r\n\t" + sql_select + "\r\n\trevert;";
                }

                syntaxEditor1.Text = sql_select;
                FillGrid(sql_select);
            }  
        }

        private void dataGridView_design_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (dataGridView_design.Rows[e.RowIndex].Cells[2].FormattedValue.ToString() == string.Empty)
            {
                e.Cancel = true;
                dataGridView_design.Rows[e.RowIndex].Cells[2].ErrorText = "Mandatory";
            }
            else
            {
                dataGridView_design.Rows[e.RowIndex].Cells[2].ErrorText = string.Empty;
            }
        }

        private void btn_def_exec_Click(object sender, EventArgs e)
        {
            ExecuteDefinition();
        }


        internal void PerformExecute()
        {
            if(tabControl1.SelectedTab.Name == "tabPage_sql")
            {
                ExecuteDefinition();
            }
            else if (tabControl1.SelectedTab.Name == "tabPage_editor")
            {
                Execute();
            }
        }

        private void dataGridView_properties_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            var prop = (DBExtendedProperty)e.Row.DataBoundItem;
            DBObjectManager.CreateExtendedProperty(prop.Owner, prop.Name, prop.Value, "");
        }

        private void dataGridView_properties_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            var prop = (DBExtendedProperty)e.Row.DataBoundItem;
            DBObjectManager.CreateExtendedProperty(prop.Owner, prop.Name, "", prop.Value);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            syntaxEditor2.Text = DBObject.GetSQL();
        }

        private void createAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBObject.Action = DB.DBAction.Create;
            if(DBObject is DBTable)
            {
                var t = DBObject as DBTable;
                foreach (var c in t.Columns)
                    c.Action = DB.DBAction.None;
            }

            syntaxEditor2.Text = DBObject.GetSQL();
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBObject.Action = DB.DBAction.Add;

            syntaxEditor2.Text = DBObject.GetSQL();
        }

        private void alterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBObject.Action = DB.DBAction.Alter;
            if (DBObject is DBTable)
            {
                var t = DBObject as DBTable;
                foreach (var c in t.Columns)
                    c.Action = DB.DBAction.Add;
            }

            syntaxEditor2.Text = DBObject.GetSQL();
        }

        private void dropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DBObject.Action = DB.DBAction.Drop;
            if (DBObject is DBTable)
            {
                var t = DBObject as DBTable;
                foreach (var c in t.Columns)
                    c.Action = DB.DBAction.Drop;
            }

            syntaxEditor2.Text = DBObject.GetSQL();
        }

        private void sELECTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(DBObject is DBContainsColumns)
            {
                var o = DBObject as DBContainsColumns;
                syntaxEditor1.Text = o.GetSelect();
            }
        }

        private void iNSERTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DBObject is DBContainsColumns)
            {
                var o = DBObject as DBContainsColumns;
                syntaxEditor1.Text = o.GetInsert();
            }
        }

        private void uPDATEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DBObject is DBContainsColumns)
            {
                var o = DBObject as DBContainsColumns;
                syntaxEditor1.Text = o.GetUpdate();
            }
        }

        private void dELETEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DBObject is DBContainsColumns)
            {
                var o = DBObject as DBContainsColumns;
                syntaxEditor1.Text = o.GetDelete();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
