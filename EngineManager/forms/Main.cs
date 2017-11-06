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

namespace EngineManager
{
    public partial class Main : Form
    {
        public List<SimpleConnection> Connections;

        public Main()
        {
            InitializeComponent();

            if(Environment.MachineName == "BOSS")
            {
                toolStripTextBox_server.Text = @"boss\sql2014";                
            }

            //load connections
            Connections = Util.ReadData(Util.AppData.Connections) as List<SimpleConnection>;
            toolStripSplitButton_quick.DropDownItems.Clear();
            foreach(var c in Connections)
            {
                var item = new ToolStripMenuItem(c.Name) { Tag = c };
                item.Click += Quick_Connect_Item_Click;
                toolStripSplitButton_quick.DropDownItems.Add(item);
            }
        }

        private void Quick_Connect_Item_Click(object sender, EventArgs e)
        {
            var c = (sender as ToolStripMenuItem).Tag as SimpleConnection;
            Connect(c.Server, c.Database, c.User, c.Password);
        }

        private DBConnection last_connection;
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var project = tree_existing.Nodes[0].Tag as DBProject;
            CreateNewForm(new DBObject() { OnAlreadyExists = DBObject.DBOnExists.PerformActionWithNoExistenceCheck, Name = "NewQuery", Connection = project.Connections[0] });

        }

        private DBObjectControl CreateNewForm(DBObject obj)
        {
            DBObjectControl childForm = null;

            //see if already open
            foreach(TabPage p in tabControl1.TabPages)
            {
                if (p.Text.Equals(obj.FullName))
                {
                    childForm = p.Tag as DBObjectControl;
                    tabControl1.SelectedTab = p;
                    break;
                }
            }

            //new?
            if (childForm == null)
            {
                childForm = new DBObjectControl();
                childForm.DBObject = obj;
                childForm.RefreshDisplay();
                childForm.ActiveObjectChanged += childForm_ActiveObjectChanged;

                //activate the new tab
                this.ActivateForm(childForm);
            }

            return childForm;
        }

        void childForm_ActiveObjectChanged(object sender, EventArgs e)
        {
            UpdatePropertyGrid(sender);
        }

        private void ActivateForm(DBObjectControl child_form)
        {
            // If child form is new and no has tabPage, 
            // create new tabPage 
            if (child_form.Tag == null)
            {
                // Add a tabPage to tabControl with child 
                TabPage tp = new TabPage(child_form.DBObject.FullName);

                //tab icon
                if (child_form.DBObject is DBSchema) tp.ImageIndex = 13;
                if (child_form.DBObject is DBTable) tp.ImageIndex = 3;
                if (child_form.DBObject is DBView) tp.ImageIndex = 17;
                if (child_form.DBObject is DBColumn) tp.ImageIndex = 4;                                     

                tp.Tag = child_form;
                tp.Parent = tabControl1;
                tabControl1.SelectedTab = tp;
                tp.Controls.Add(child_form); //show it
                child_form.Parent = tp;
                child_form.Dock = DockStyle.Fill;
                child_form.Tag = tp;
            }
            
        }

        private void toolStripButton_connect_Click(object sender, EventArgs e)
        {
            Connect(toolStripTextBox_server.Text, toolStripTextBox_database.Text, toolStripTextBox_user.Text, toolStripTextBox_password.Text);
        }

        public void Connect(string server, string db, string user, string password)
        {
            DBProject project;
            if (tree_existing.Nodes.Count == 0)
            {
                project = new DBProject { Name = "Engine Manager" };
                tree_existing.Nodes.Add(project.GetTree());
                //var command_list = new BindingList<DBCommand>(project.Commands);
                //var pending_commands = new BindingSource(project, "Commands");
                dataGridView_pending_commands.DataSource = project.Commands;
            }
            else
            {
                project = tree_existing.Nodes[0].Tag as DBProject;
            }

            var new_connection = new DBConnection()
            {
                Server = server,
                InitialDatabase = db,
                User = user,
                Password = password,
                Project = project
            };

            this.last_connection = new_connection;

            project.Connections.Add(new_connection);

            var new_conn = GetExistingDatabase(new_connection);

            var project_node = tree_existing.Nodes[0];

            project_node.Nodes.Add(new_conn);
            if (tree_existing.Nodes.Count > 0)
                AttachTreeNodesContextMenus(project_node);
            project_node.Expand();
        }

        private TreeNode GetExistingDatabase(DBConnection conn)
        {

            DBConnectionManager.Refresh(conn);

            var root = new TreeNode(conn.Server + " (" + conn.User + ")") { Tag = conn };

            root.Nodes.Add(conn.GetTree());
            root.Expand();
            IconizeTreeNodes(root);

            return root;
        }

        private void AttachTreeNodesContextMenus(TreeNode node)
        {
            if (node.TreeView == tree_existing) //existing tree
            {
                if (node.Tag is DBTable)
                    node.ContextMenuStrip = mnu_table;
                else if (node.Tag is DBColumn)
                    node.ContextMenuStrip = mnu_column;
                else if (node.Tag is DBFKConstraint)
                    node.ContextMenuStrip = mnu_fk;
                else if (node.Tag is DB)
                    node.ContextMenuStrip = mnu_db;
                else if (node.Tag is DBConnection)
                    node.ContextMenuStrip = mnu_connection;
                else if (node.Tag is DBRole)
                    node.ContextMenuStrip = mnu_role;
                else if (node.Tag is DBSchema)
                    node.ContextMenuStrip = mnu_schema;
                
                    //last resort
                else if (node.Tag is DBObject)
                    node.ContextMenuStrip = mnu_default;
                
                
            }

            foreach (TreeNode n in node.Nodes)
            {
                AttachTreeNodesContextMenus(n);
            }
        }

        private void FindAndSelectTreeNode(TreeNode n, object obj)
        {
            if (n.Tag == obj)
                tree_existing.SelectedNode = n;
            else
                foreach (TreeNode sn in n.Nodes)
                    FindAndSelectTreeNode(sn, obj);
        }

        private void IconizeTreeNodes(TreeNode n)
        {

            AttachTreeNodesImages(n);
            foreach (TreeNode sn in n.Nodes)
                IconizeTreeNodes(sn);
        }

        private void AttachTreeNodesImages(TreeNode n)
        {
            if (n.Tag != null)
            {
                if (n.Tag is DBProject) n.ImageIndex = 2;
                else if (n.Tag is DBConnection) n.ImageIndex = 12;
                else if (n.Tag is DB) n.ImageIndex = 7;
                else if (n.Tag is DBDatabaseTrigger) n.ImageIndex = 22;
                else if (n.Tag is DBSchema) n.ImageIndex = 13;
                else if (n.Tag is DBUserDataType) n.ImageIndex = 15;
                else if (n.Tag is DBFunction) n.ImageIndex = 16;
                else if (n.Tag is DBTable) n.ImageIndex = 3;
                else if (n.Tag is DBView) n.ImageIndex = 17;
                else if (n.Tag is DBColumn)
                {
                    n.ImageIndex = 4;
                    if ((n.Tag as DBColumn).Identity != null)
                        n.ImageIndex = 9;
                }
                else if (n.Tag is DBTrigger)
                {
                    var t = (n.Tag as DBTrigger);

                    if (t.TriggerAfterDelete && t.TriggerAfterInsert || t.TriggerAfterDelete && t.TriggerAfterUpdate || t.TriggerAfterInsert && t.TriggerAfterUpdate)
                        n.ImageIndex = 14;
                    else if (t.TriggerAfterDelete)
                        n.ImageIndex = 18;
                    else if (t.TriggerAfterInsert)
                        n.ImageIndex = 19;
                    else if (t.TriggerAfterUpdate)
                        n.ImageIndex = 20;
                    else
                        n.ImageIndex = 22;
                }
                else if (n.Tag is DBConstraint)
                {
                    switch ((n.Tag as DBConstraint).ConstraintType)
                    {
                        case DBConstraint.DBConstraintTypes.PrimaryKey: n.ImageIndex = 11; break;
                        case DBConstraint.DBConstraintTypes.ForeignKey: n.ImageIndex = 8; break;
                        case DBConstraint.DBConstraintTypes.Unique: n.ImageIndex = 5; break;
                        case DBConstraint.DBConstraintTypes.Check: n.ImageIndex = 5; break;
                    }
                }

            }
            else
                n.ImageIndex = 2;//folder

            n.SelectedImageIndex = n.ImageIndex;
        }

        private void tree_existing_AfterExpand(object sender, TreeViewEventArgs e)
        {
            //if dbobject, get its sql
            //tree_existing.SelectedNode = e.Node;
            if (e.Node.Tag != null && typeof(IRefreshableObject).IsAssignableFrom(e.Node.Tag.GetType()))
            {
                //(e.Node.Tag as IRefreshableObject).Refresh();
                //UpdatePropertyGrid(e.Node.Tag);
            //    ShowNode(e.Node);

                
            }


        }

        private void UpdatePropertyGrid(object o)
        {
            propertyGrid1.SelectedObject = o;
        }

        private void ShowNode(TreeNode node, bool refresh = false, bool open = true)
        {
            if (node == null)
                return;

            if (node.Tag is DBObject)
            {
                
                if (((node.Tag as DBObject).State == DBObject.DBObjectState.None || refresh)) 
                {
                    DBObjectManager.Refresh((node.Tag as DBObject)); //only refresh when needed or forced
                    var db_obj = (node.Tag as IGenerateTreeNode);

                    node.Nodes.Clear();
                    foreach (TreeNode n in db_obj.GetTree().Nodes)
                        node.Nodes.Add(n);
                    
                    IconizeTreeNodes(node);
                    AttachTreeNodesContextMenus(node);
                }
                //else
                //{
                //    if(node.Tag is IContainsColumns) //refresh tables if their columns collection is empty
                //    {
                //        if ((node.Tag as IContainsColumns).Columns.Count == 0)
                //        {
                //            (node.Tag as IRefreshableObject).Refresh();
                //        }
                //    }
                //}
            }

            UpdatePropertyGrid(node.Tag);

            if (node.Tag is DBObject)
            {
                if (refresh || open)
                {
                    var childForm = CreateNewForm(node.Tag as DBObject);

                    //update the details view of the object
                    if (refresh)
                    {
                        //add tab

                        childForm.RefreshDisplay();
                    }
                }
                   
            }

            if (node.Tag is IGenerateTreeNode)
            {
                //reload nodes
                /*var db_obj = (node.Tag as IGenerateTreeNode);

                node.Nodes.Clear();
                foreach (TreeNode n in db_obj.GetTree().Nodes)
                    node.Nodes.Add(n);*/

                
            }
        }

        private void tree_existing_AfterSelect(object sender, TreeViewEventArgs e)
        {           
            ShowNode(e.Node, false, false);          
        }

        private void tree_existing_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //select on any click (left or right click)
            e.Node.TreeView.SelectedNode = e.Node;
        }

        private void openInNewTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var node = tree_existing.SelectedNode;
            //if(node != null && node.Tag != null && typeof(DBObject).IsAssignableFrom(node.Tag.GetType()))
            //{


            //}            
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((tabControl1.SelectedTab != null) && (tabControl1.SelectedTab.Tag != null))
            {
                //(tabControl1.SelectedTab.Tag as Form).Select();
                //ResizeActiveTab();

                //update the properties window
                UpdatePropertyGrid((tabControl1.SelectedTab.Tag as DBObjectControl).DBObject);
            }
        }
        public DBObjectControl ActiveDBObjectControl
        {
            get
            {
                if(tabControl1.SelectedTab != null)
                {
                    if(tabControl1.SelectedTab.Tag is DBObjectControl)
                    {
                        return tabControl1.SelectedTab.Tag as DBObjectControl;
                    }
                }

                return null;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                if (ActiveDBObjectControl != null)
                {

                    ActiveDBObjectControl.PerformExecute();
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }





        private void toolStripMenuItem_close_Click(object sender, EventArgs e)
        {
            var tag = ((sender as ToolStripItem).Owner as ContextMenuStrip).Tag;
            if (tag != null)
            {
                var tab_page = ((sender as ToolStripItem).Owner as ContextMenuStrip).Tag as TabPage;
                (tab_page.Tag as DBObjectControl).Dispose();
                tab_page.Dispose();
            }
            
        }



        private void tabControl1_MouseClick(object sender, MouseEventArgs e)
        {
            //show menu for closing tab..
            for (int i = 0; i < tabControl1.TabCount; ++i)
            {
                Rectangle r = tabControl1.GetTabRect(i);
                if (r.Contains(e.Location) /* && it is the header that was clicked*/)
                {
                    var tab_page = tabControl1.TabPages[i];
                    if (e.Button == MouseButtons.Right)
                    {
                        // Change slected index, get the page, create contextual menu
                        mnu_tab.Tag = tab_page;
                        mnu_tab.Show(tabControl1, e.Location);
                        break;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                    {
                        //close on middle click
                        (tab_page.Tag as DBObjectControl).Dispose();
                        tab_page.Dispose();
                        break;
                    }
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton_save_Click(object sender, EventArgs e)
        {
            if(tree_existing.Nodes[0] != null)
            {
                if(propertyGrid1.SelectedObject is DBObject)
                {
                    var current_db_obj = propertyGrid1.Tag as DBObject;

                    var pending_commands = (tree_existing.Nodes[0].Tag as DBProject).Commands.Where(x=>x.State == DBCommand.CommandState.New);

                    var this_object_pending_commands = pending_commands.Where(x => x.Owner == propertyGrid1.SelectedObject as DBObject).ToList();

                    //var executed_successfully = new List<DBCommand>();

                    foreach(var c in this_object_pending_commands)
                    {
                        DBProjectManager.Execute(c);
                        //if(cmd.State == DBCommand.CommandState.Executed)
                        //{
                        //    executed_successfully.Add(cmd);
                        //}
                    }

                    //foreach(var executed_cmd in executed_successfully)
                    //{
                    //    this_object_pending_commands.Remove(executed_cmd);
                    //}
                }
            }

            
            
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode, true); //force refresh
            dataGridView_pending_commands.Refresh();
        }

        private void toolStripMenuItem_close_all_Click(object sender, EventArgs e)
        {
            var tag = ((sender as ToolStripItem).Owner as ContextMenuStrip).Tag;
            if (tag != null)
            {
                var tab_page = ((sender as ToolStripItem).Owner as ContextMenuStrip).Tag as TabPage;

                foreach(TabPage tp in tabControl1.TabPages)
                {
                    if (tp != tab_page)
                    {
                        (tp.Tag as DBObjectControl).Dispose();
                        tp.Dispose();
                    }
                }                
            }
        }

        private void refreshToolStripMenuItem_refresh_Click(object sender, EventArgs e)
        {
            var tag = ((sender as ToolStripItem).Owner as ContextMenuStrip).Tag;
            if (tag != null)
            {
                var tab_page = tag as TabPage;
                var control = (tab_page.Tag as DBObjectControl);
                DBObjectManager.Refresh(control.DBObject);
                control.RefreshDisplay();
            }
        }

        private void toolStripButton_save_all_Click(object sender, EventArgs e)
        {
            listBox_messages.Items.Clear();

            var project = (tree_existing.Nodes[0].Tag as DBProject);
            foreach (var c in project.Connections)
            {
                c.sql_messages.Clear();
            }

            var pending_commands = project.Commands.Where(x => x.State == DBCommand.CommandState.New).ToList();
            var executed_successfully = new List<DBCommand>();

            foreach (var cmd in pending_commands)
            {
                DBProjectManager.Execute(cmd);
                listBox_messages.Items.AddRange(cmd.Owner.Connection.sql_messages.ToArray());

                if (cmd.State == DBCommand.CommandState.Executed)
                {
                    executed_successfully.Add(cmd);
                }
            }

            foreach (var executed_cmd in executed_successfully)
            {
                project.Commands.Remove(executed_cmd);
            }

        }

        private void toolStripButton_cancel_all_Click(object sender, EventArgs e)
        {
            var project = (tree_existing.Nodes[0].Tag as DBProject);
            project.Commands.Clear();

            //todo, revert instead..
        }

        private void mnu_item_create_select_policy_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var policy_view = DBTableManager.CreatePolicy(db_table, DBTable.PolicyTypes.Select);
            CreateNewForm(policy_view);
        }

        private void mnu_item_create_update_policy_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var policy_view = DBTableManager.CreatePolicy(db_table, DBTable.PolicyTypes.Update);
            CreateNewForm(policy_view);
        }

        private void mnu_item_create_delete_policy_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var policy_view = DBTableManager.CreatePolicy(db_table, DBTable.PolicyTypes.Delete);
            CreateNewForm(policy_view);
        }

        private void mnu_item_create_extension_view_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var view = DBTableManager.CreateView(db_table, DBTable.ViewTypes.Views);
            CreateNewForm(view);
        }

        private void mnu_item_create_computed_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var view = DBTableManager.CreateView(db_table, DBTable.ViewTypes.Computed);
            CreateNewForm(view);
        }

        private void mnu_item_create_secure_view_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBView view = DBTableManager.CreateSecureView(db_table);
            CreateNewForm(view);
        }

        private void toolStripButton_parent_properties_Click(object sender, EventArgs e)
        {
            if (this.ActiveDBObjectControl != null)
                UpdatePropertyGrid(this.ActiveDBObjectControl.DBObject);
        }

        private void fontTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new TestForm();
            form.ShowDialog();
        }

        private void mnu_item_manage_fields_Click(object sender, EventArgs e)
        {
            var table = tree_existing.SelectedNode.Tag as DBTable;
            var form = new ManageRelatedColumns { Table = table };
            form.ShowDialog();
        }

        private void toolStripButton_sao_alzeit_Click(object sender, EventArgs e)
        {
            //tables
                //auto assign row properties e.g. row_date, row_title, badge...
            if(SelectedDBObject is DBTable)
            {
                var obj = SelectedDBObject as DBTable;
                var image_column = obj.CombinedColumns.Where(x => x.UserDataType.ToString().ToLower().Contains("image")).FirstOrDefault();
                if(image_column != null)
                {
                    obj.RowImage = image_column;
                }

                var icon_column = obj.CombinedColumns.Where(x => x.UserDataType.ToString().ToLower().Contains("icon")).FirstOrDefault();
                if (icon_column != null)
                {
                    obj.RowIconClass = icon_column;
                }

                var color_column = obj.CombinedColumns.Where(x => x.UserDataType.ToString().ToLower().Contains("color")).FirstOrDefault();
                if (color_column != null)
                {
                    obj.RowColor = color_column;
                }
            }
            
        }

        private void dataGridView_pending_commands_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            var form = new DBCommandPreview()
            {
                Command = (dataGridView_pending_commands.DataSource as BindingList<DBCommand>)[e.RowIndex] as DBCommand
            };
            form.ShowDialog();
        }

        private void mnu_db_disable_triggers_Click(object sender, EventArgs e)
        {
            var db = SelectedDBObject as DB;
            DBManager.DisableAllTriggers(db);

        }

        private void mnu_db_enable_triggers_Click(object sender, EventArgs e)
        {
            var db = SelectedDBObject as DB;
            DBManager.EnableAllTriggers(db);
        }

        private void toolStripComboBox_tree_font_SelectedIndexChanged(object sender, EventArgs e)
        {
            tree_existing.Font = new Font("Microsoft Sans Serif", float.Parse(toolStripComboBox_tree_font.SelectedItem.ToString()));
            tree_existing.ItemHeight = Convert.ToInt16((tree_existing.Font.Size) * 2);
            //propertyGrid1.Font = tree_existing.Font;
        }

        private void tree_existing_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //ShowNode(e.Node);
        }

        private void mnu_default_refresh_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode, true);
        }

        public DBObject SelectedDBObject
        {
            get
            {
                if (tree_existing.SelectedNode.Tag is DBObject)
                    return tree_existing.SelectedNode.Tag as DBObject;
                return null;
            }
        }

        private void mnu_default_open_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode);
        }

        private void managePermissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ManageObjectPermissions();
            form.DBObject = SelectedDBObject as DBSchemaObject;
            form.ShowDialog();
        }

        private void managePermissionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new ManageObjectPermissions();
            form.DBObject = SelectedDBObject  as DBSchemaObject;
            form.ShowDialog();
        }

        private void manageRelationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ManageRelationsForm();
            form.Table = SelectedDBObject as DBTable;
            form.ShowDialog();
        }

        private void migrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Migration();
            form.Connection = this.last_connection;
            form.ShowDialog();
        }

        private void addBusinessProcedureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var proc = DBTableManager.CreateBusinessProcedure(db_table, DBTable.ProcedureTypes.Business);
            CreateNewForm(proc);
        }

        private void manageActionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var db_table = SelectedDBObject as DBTable;
            //var form = new ManageActionsForm();
            //form.DBTable = db_table;
            //form.ShowDialog();
        }

        private void manageCheckConstraintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            var form = new ManageCheckConstraints();
            form.DBTable = db_table;
            form.ShowDialog();
        }

        private void disableTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.DisableTriggers(db_table);
        }

        private void enableTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.EnableTriggers(db_table);
        }

        private void addIconClassColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.AddIconClassColumn(db_table);
            
        }

        private void addLabelTypeColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.AddColorColumn(db_table);
        }

        private void addDisplayOrderColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.AddDisplayOrderColumn(db_table);
        }

        private void refreshCacheToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.RefreshCache(db_table);
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var db = SelectedDBObject as DB;
            DBManager.Refresh(db);
            var db_node = tree_existing.SelectedNode;
            db_node.Nodes.Clear();
            foreach (TreeNode n in db.GetTree().Nodes)
                db_node.Nodes.Add(n);

            IconizeTreeNodes(db_node);
        }

        private void mnu_item_create_generic_trigger_Click(object sender, EventArgs e)
        {
            var db_table = SelectedDBObject as DBTable;
            DBTableManager.CreateGenericTrigger(db_table);
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var role = SelectedDBObject as DBRole;
            var form = new ManageRolesForm();
            form.Role = role;
            form.ShowDialog();
        }

        private void pMVDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(@"sv800617\sql2012", "fe", "PTIU", "Migrate.12");
        }

        private void pMVTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(@"sv800507\fe", "fe", "PTIU", "Migrate.12");
        }

        private void pMPSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Connect(@"sv800545\fe", "fe", "PTIU", "Migrate.12");
        }

        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree_existing.Nodes.Remove(tree_existing.SelectedNode);
        }

        private void toolStripButton_collapse_Click(object sender, EventArgs e)
        {
            tree_existing.CollapseAll();
            tree_existing.Nodes[0].Expand();
        }

        private void addNewColumnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var table = SelectedDBObject as DBTable;
            var form = new AddNewColumnForm();
            form.Table = table;
            form.ShowDialog();
        }

        private void createFKToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new SelectTableForm();
            form.FormClosing += form_FormClosing;

            form.Mode = SelectTableForm.Modes.Column;
            form.DBObject = (SelectedDBObject as DBColumn).Parent;
            form.ShowDialog();

            
        }

        void form_FormClosing(object sender, FormClosingEventArgs e)
        {
            var ref_column = (sender as SelectTableForm).SelectedObject as DBColumn;
            (SelectedDBObject as DBColumn).CreateFK(ref_column);
        }

        private void toolStripSplitButton_quick_ButtonClick(object sender, EventArgs e)
        {
            Connect(@"sv800617\sql2012", "fe", "PTIU", "Migrate.12");
        }

        private void lookupTableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var schema = SelectedDBObject as DBSchema;
            var lookup_table = new DBTable
            {
                Name = "NewTable",
                Action = DB.DBAction.Create,
                Connection = schema.Connection,
                State = DBObject.DBObjectState.New,
                OnAlreadyExists = DBObject.DBOnExists.DoNothingElsePerformAction,
                Schema = schema                
            };

            lookup_table.Columns.AddRange(new List<DBColumn> {
                    new DBColumn { DisplayOrder = "0", Name = "id", DataType = DB.DBDataType.nvarchar, Length = 50, Nullable = false},
                    new DBColumn { DisplayOrder = "1", Name = "name", DataType = DB.DBDataType.nvarchar, Length = 50, Nullable = false},
                    new DBColumn { DisplayOrder = "2", Name = "description", DataType = DB.DBDataType.nvarchar, Length = 100, Nullable = true},
                    new DBColumn { DisplayOrder = "3", Name = "icon", DataType = DB.DBDataType.nvarchar, Length = 50, Nullable = true, UserDataType = "iconclass"},
                    new DBColumn { DisplayOrder = "4", Name = "color", DataType = DB.DBDataType.nvarchar, Length = 50, Nullable = true, UserDataType = "color"},
                    new DBColumn { DisplayOrder = "5", Name = "display_order", DataType = DB.DBDataType.@int, Length = 4, Nullable = true, UserDataType = "displayorder"}
                });

            var form = new InputForm() { Object = lookup_table };
            form.FormClosing += NewLookupTableFormClosing;
            form.ShowDialog();
            
        }

        private void NewLookupTableFormClosing(object sender, FormClosingEventArgs e)
        {
            var form = (sender as InputForm);
            if (form.DialogResult == DialogResult.Cancel)
                return;

            var lookup_table = form.Object as DBTable;

            var sql = lookup_table.GetSQL();

            lookup_table.Connection.Project.Commands.Add(new DBCommand { Sql = sql, Owner = lookup_table, Description = "Create New Lookup Table" });

            

            //set custom properties
            lookup_table.RowIconClass = lookup_table.Columns.Where(c => c.UserDataType == "iconclass").FirstOrDefault();
            lookup_table.RowColor = lookup_table.Columns.Where(c => c.UserDataType == "color").FirstOrDefault();
            lookup_table.DefaultSort = lookup_table.Columns.Where(c => c.UserDataType == "displayorder").FirstOrDefault();

            //pk
            var pk_col = lookup_table.Columns.Where(c => c.Name == "id").FirstOrDefault();
            pk_col.IsPrimaryKey = true;

            //execute the commands..
            foreach(var c in lookup_table.Connection.Project.Commands.Where(c=> c.Owner == lookup_table).ToList())
            {
                DBProjectManager.Execute(c);
            }
            DBTableManager.Refresh(lookup_table);
            //show the designer..
            CreateNewForm(lookup_table);
        }

        private void ResetConnections()
        {
            var connections = new List<SimpleConnection>
            {
                new SimpleConnection{ Name = "Development", Server = @"sv800617\sql2012",   Database = "fe", User = "PTIU", Password = "Migrate.12" },
                new SimpleConnection{ Name = "Test",        Server = @"sv800507\fe",        Database = "fe", User = "PTIU", Password = "Migrate.12" },
                new SimpleConnection{ Name = "Production",  Server = @"sv800545\fe",        Database = "fe", User = "PTIU", Password = "Migrate.12" },
                new SimpleConnection{ Name = "Home",        Server = @"boss\sql2014",       Database = "fe", User = "PTIU", Password = "Migrate.12" }
            };

            Util.SaveData(Util.AppData.Connections, connections);
        }

        private void permissionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ManageRolePermissions();
            form.InitialRole = SelectedDBObject as DBRole;
            form.RefreshView();
            form.ShowDialog();
        }

        private void convertToLookupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new ConvertToLookupForm();
            form.Column = SelectedDBObject as DBColumn;
            if (form.Column.Parent is DBView)
                MessageBox.Show("View columns cannot be used to generate a lookup.");
            form.ShowDialog();
        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode);
        }

        private void openToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ShowNode(tree_existing.SelectedNode);
        }

        private void compressionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new CompressForm();
            form.ShowDialog();
        }
    }

    public class SimpleConnection
    {
        public string Name { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
