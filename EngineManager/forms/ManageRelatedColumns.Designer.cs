namespace EngineManager
{
    partial class ManageRelatedColumns
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("id");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("title");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("hr.organizations (organization_id)", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageRelatedColumns));
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("id");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("name");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("llkb.originators", new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode5});
            ActiproSoftware.SyntaxEditor.Document document1 = new ActiproSoftware.SyntaxEditor.Document();
            ActiproSoftware.SyntaxEditor.Document document2 = new ActiproSoftware.SyntaxEditor.Document();
            ActiproSoftware.SyntaxEditor.Document document3 = new ActiproSoftware.SyntaxEditor.Document();
            this.label_parent_tables = new System.Windows.Forms.Label();
            this.label_sub_tables = new System.Windows.Forms.Label();
            this.treeView_parents = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.treeView_sub = new System.Windows.Forms.TreeView();
            this.cmb_aggregation = new System.Windows.Forms.ComboBox();
            this.label_aggregation = new System.Windows.Forms.Label();
            this.btn_add_parent = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label_preview = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.lbl_source = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btn_add_sub = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.syntaxEditor1 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.label_sort = new System.Windows.Forms.Label();
            this.cmb_sort_column = new System.Windows.Forms.ComboBox();
            this.label_sort_dir = new System.Windows.Forms.Label();
            this.cmb_sort_dir = new System.Windows.Forms.ComboBox();
            this.syntaxEditor2 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.treeView_computed = new System.Windows.Forms.TreeView();
            this.label_current_table = new System.Windows.Forms.Label();
            this.treeView_current = new System.Windows.Forms.TreeView();
            this.btn_add_current = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage_added_columns = new System.Windows.Forms.TabPage();
            this.treeView_added = new System.Windows.Forms.TreeView();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_added_new_computation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_added_new_question = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_col_delete = new System.Windows.Forms.ToolStripButton();
            this.panel_bottom_added_columns = new System.Windows.Forms.Panel();
            this.tabPage_computed_columns = new System.Windows.Forms.TabPage();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_computed = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_computed_delete = new System.Windows.Forms.ToolStripButton();
            this.panel_computed_bottom = new System.Windows.Forms.Panel();
            this.splitContainer_center = new System.Windows.Forms.SplitContainer();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.listBox_questions = new System.Windows.Forms.ListBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_question = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label_sec_questions = new System.Windows.Forms.Label();
            this.treeView_policies = new System.Windows.Forms.TreeView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_policy = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_new_node = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_up = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_down = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_orize = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_andize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_delete_node = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton_policy_definition = new System.Windows.Forms.ToolStripButton();
            this.syntaxEditor3 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label_policies = new System.Windows.Forms.Label();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tab_general = new System.Windows.Forms.TabPage();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.tab_operations = new System.Windows.Forms.TabPage();
            this.toolStrip7 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_refresh_operation_policies = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox_select_policy = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox_delete_policy = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox_update_policy = new System.Windows.Forms.ComboBox();
            this.tab_validations = new System.Windows.Forms.TabPage();
            this.dataGridView_validators = new System.Windows.Forms.DataGridView();
            this.toolStrip6 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_validation = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete_validation = new System.Windows.Forms.ToolStripButton();
            this.tab_commands = new System.Windows.Forms.TabPage();
            this.tabControl4 = new System.Windows.Forms.TabControl();
            this.tab_insert_commands = new System.Windows.Forms.TabPage();
            this.listBox_insert_commands = new System.Windows.Forms.ListBox();
            this.toolStrip9 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_insert_command = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete_insert_command = new System.Windows.Forms.ToolStripButton();
            this.tab_update_commands = new System.Windows.Forms.TabPage();
            this.listBox_update_commands = new System.Windows.Forms.ListBox();
            this.toolStrip8 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_update_command = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete_update_command = new System.Windows.Forms.ToolStripButton();
            this.tab_delete_commands = new System.Windows.Forms.TabPage();
            this.listBox_delete_commands = new System.Windows.Forms.ListBox();
            this.toolStrip10 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_delete_command = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete_delete_command = new System.Windows.Forms.ToolStripButton();
            this.tab_basic_procedures = new System.Windows.Forms.TabPage();
            this.listBox_basic_procedures = new System.Windows.Forms.ListBox();
            this.toolStrip11 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_basic_procedure = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_delete_basic_procedure = new System.Windows.Forms.ToolStripButton();
            this.tab_actions = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel_actions = new System.Windows.Forms.FlowLayoutPanel();
            this.toolStrip5 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_new_action = new System.Windows.Forms.ToolStripButton();
            this.tab_workflow = new System.Windows.Forms.TabPage();
            this.button_manage_workflow = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel_syntax_top = new System.Windows.Forms.Panel();
            this.comboBox_syntax_source = new System.Windows.Forms.ComboBox();
            this.label_syntax_source = new System.Windows.Forms.Label();
            this.panel_top = new System.Windows.Forms.Panel();
            this.panel_bottom = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_preview_computed = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage_added_columns.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.tabPage_computed_columns.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_center)).BeginInit();
            this.splitContainer_center.Panel1.SuspendLayout();
            this.splitContainer_center.Panel2.SuspendLayout();
            this.splitContainer_center.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tab_general.SuspendLayout();
            this.tab_operations.SuspendLayout();
            this.toolStrip7.SuspendLayout();
            this.tab_validations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_validators)).BeginInit();
            this.toolStrip6.SuspendLayout();
            this.tab_commands.SuspendLayout();
            this.tabControl4.SuspendLayout();
            this.tab_insert_commands.SuspendLayout();
            this.toolStrip9.SuspendLayout();
            this.tab_update_commands.SuspendLayout();
            this.toolStrip8.SuspendLayout();
            this.tab_delete_commands.SuspendLayout();
            this.toolStrip10.SuspendLayout();
            this.tab_basic_procedures.SuspendLayout();
            this.toolStrip11.SuspendLayout();
            this.tab_actions.SuspendLayout();
            this.toolStrip5.SuspendLayout();
            this.tab_workflow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel_syntax_top.SuspendLayout();
            this.panel_top.SuspendLayout();
            this.panel_bottom.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label_parent_tables
            // 
            this.label_parent_tables.AutoSize = true;
            this.label_parent_tables.Location = new System.Drawing.Point(10, 130);
            this.label_parent_tables.Name = "label_parent_tables";
            this.label_parent_tables.Size = new System.Drawing.Size(73, 13);
            this.label_parent_tables.TabIndex = 1;
            this.label_parent_tables.Text = "Parent Tables";
            // 
            // label_sub_tables
            // 
            this.label_sub_tables.AutoSize = true;
            this.label_sub_tables.Location = new System.Drawing.Point(7, 326);
            this.label_sub_tables.Name = "label_sub_tables";
            this.label_sub_tables.Size = new System.Drawing.Size(61, 13);
            this.label_sub_tables.TabIndex = 3;
            this.label_sub_tables.Text = "Sub Tables";
            // 
            // treeView_parents
            // 
            this.treeView_parents.FullRowSelect = true;
            this.treeView_parents.HideSelection = false;
            this.treeView_parents.ImageKey = "folder";
            this.treeView_parents.ImageList = this.imageList1;
            this.treeView_parents.Location = new System.Drawing.Point(10, 144);
            this.treeView_parents.Name = "treeView_parents";
            treeNode1.Name = "Node1";
            treeNode1.Text = "id";
            treeNode2.Name = "Node2";
            treeNode2.Text = "title";
            treeNode3.Name = "Node0";
            treeNode3.Text = "hr.organizations (organization_id)";
            this.treeView_parents.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode3});
            this.treeView_parents.SelectedImageIndex = 0;
            this.treeView_parents.Size = new System.Drawing.Size(254, 179);
            this.treeView_parents.TabIndex = 4;
            this.treeView_parents.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_parents_NodeMouseClick);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "arrow.png");
            this.imageList1.Images.SetKeyName(1, "column");
            this.imageList1.Images.SetKeyName(2, "connect.gif");
            this.imageList1.Images.SetKeyName(3, "constraint.gif");
            this.imageList1.Images.SetKeyName(4, "added_column");
            this.imageList1.Images.SetKeyName(5, "database.gif");
            this.imageList1.Images.SetKeyName(6, "fk.png");
            this.imageList1.Images.SetKeyName(7, "folder");
            this.imageList1.Images.SetKeyName(8, "function.png");
            this.imageList1.Images.SetKeyName(9, "id.png");
            this.imageList1.Images.SetKeyName(10, "lightning.png");
            this.imageList1.Images.SetKeyName(11, "loop.png");
            this.imageList1.Images.SetKeyName(12, "operation.png");
            this.imageList1.Images.SetKeyName(13, "pk.png");
            this.imageList1.Images.SetKeyName(14, "procedure.gif");
            this.imageList1.Images.SetKeyName(15, "project.png");
            this.imageList1.Images.SetKeyName(16, "schema.png");
            this.imageList1.Images.SetKeyName(17, "table");
            this.imageList1.Images.SetKeyName(18, "tr_delete.png");
            this.imageList1.Images.SetKeyName(19, "tr_insert.gif");
            this.imageList1.Images.SetKeyName(20, "tr_update.png");
            this.imageList1.Images.SetKeyName(21, "trigger.png");
            this.imageList1.Images.SetKeyName(22, "type.gif");
            this.imageList1.Images.SetKeyName(23, "unique.png");
            this.imageList1.Images.SetKeyName(24, "view.gif");
            this.imageList1.Images.SetKeyName(25, "warning.gif");
            // 
            // treeView_sub
            // 
            this.treeView_sub.FullRowSelect = true;
            this.treeView_sub.HideSelection = false;
            this.treeView_sub.ImageKey = "folder";
            this.treeView_sub.ImageList = this.imageList1;
            this.treeView_sub.Location = new System.Drawing.Point(10, 342);
            this.treeView_sub.Name = "treeView_sub";
            treeNode4.Name = "Node1";
            treeNode4.Text = "id";
            treeNode5.Name = "Node2";
            treeNode5.Text = "name";
            treeNode6.Name = "Node0";
            treeNode6.Text = "llkb.originators";
            this.treeView_sub.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6});
            this.treeView_sub.SelectedImageIndex = 0;
            this.treeView_sub.Size = new System.Drawing.Size(254, 183);
            this.treeView_sub.TabIndex = 4;
            this.treeView_sub.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_sub_AfterSelect);
            // 
            // cmb_aggregation
            // 
            this.cmb_aggregation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_aggregation.FormattingEnabled = true;
            this.cmb_aggregation.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.cmb_aggregation.Location = new System.Drawing.Point(87, 540);
            this.cmb_aggregation.Name = "cmb_aggregation";
            this.cmb_aggregation.Size = new System.Drawing.Size(177, 21);
            this.cmb_aggregation.TabIndex = 5;
            // 
            // label_aggregation
            // 
            this.label_aggregation.AutoSize = true;
            this.label_aggregation.Location = new System.Drawing.Point(8, 543);
            this.label_aggregation.Name = "label_aggregation";
            this.label_aggregation.Size = new System.Drawing.Size(64, 13);
            this.label_aggregation.TabIndex = 3;
            this.label_aggregation.Text = "Aggregation";
            // 
            // btn_add_parent
            // 
            this.btn_add_parent.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_add_parent.Image = ((System.Drawing.Image)(resources.GetObject("btn_add_parent.Image")));
            this.btn_add_parent.Location = new System.Drawing.Point(272, 215);
            this.btn_add_parent.Name = "btn_add_parent";
            this.btn_add_parent.Size = new System.Drawing.Size(33, 30);
            this.btn_add_parent.TabIndex = 6;
            this.btn_add_parent.UseVisualStyleBackColor = true;
            this.btn_add_parent.Click += new System.EventHandler(this.btn_add_parent_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 13);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1850, 159);
            this.dataGridView1.TabIndex = 8;
            // 
            // label_preview
            // 
            this.label_preview.AutoSize = true;
            this.label_preview.Dock = System.Windows.Forms.DockStyle.Top;
            this.label_preview.Location = new System.Drawing.Point(0, 0);
            this.label_preview.Name = "label_preview";
            this.label_preview.Size = new System.Drawing.Size(45, 13);
            this.label_preview.TabIndex = 1;
            this.label_preview.Text = "Preview";
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button4.Location = new System.Drawing.Point(371, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 6;
            this.button4.Text = "OK";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // lbl_source
            // 
            this.lbl_source.AutoSize = true;
            this.lbl_source.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_source.Location = new System.Drawing.Point(9, 9);
            this.lbl_source.Name = "lbl_source";
            this.lbl_source.Size = new System.Drawing.Size(396, 20);
            this.lbl_source.TabIndex = 1;
            this.lbl_source.Text = "Manage Related Columns for table: llkb.Lessons";
            // 
            // button5
            // 
            this.button5.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button5.Location = new System.Drawing.Point(452, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 6;
            this.button5.Text = "Cancel";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(3, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(135, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "Preview Cached View";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.btn_preview_Click);
            // 
            // btn_add_sub
            // 
            this.btn_add_sub.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_add_sub.Image = ((System.Drawing.Image)(resources.GetObject("btn_add_sub.Image")));
            this.btn_add_sub.Location = new System.Drawing.Point(272, 414);
            this.btn_add_sub.Name = "btn_add_sub";
            this.btn_add_sub.Size = new System.Drawing.Size(33, 34);
            this.btn_add_sub.TabIndex = 9;
            this.btn_add_sub.UseVisualStyleBackColor = true;
            this.btn_add_sub.Click += new System.EventHandler(this.btn_add_sub_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // syntaxEditor1
            // 
            this.syntaxEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor1.Document = document1;
            this.syntaxEditor1.Font = new System.Drawing.Font("Courier New", 8F);
            this.syntaxEditor1.LineNumberMarginVisible = true;
            this.syntaxEditor1.Location = new System.Drawing.Point(3, 3);
            this.syntaxEditor1.Name = "syntaxEditor1";
            this.syntaxEditor1.Size = new System.Drawing.Size(362, 235);
            this.syntaxEditor1.TabIndex = 10;
            // 
            // label_sort
            // 
            this.label_sort.AutoSize = true;
            this.label_sort.Location = new System.Drawing.Point(8, 567);
            this.label_sort.Name = "label_sort";
            this.label_sort.Size = new System.Drawing.Size(64, 13);
            this.label_sort.TabIndex = 3;
            this.label_sort.Text = "Sort Column";
            // 
            // cmb_sort_column
            // 
            this.cmb_sort_column.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sort_column.FormattingEnabled = true;
            this.cmb_sort_column.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.cmb_sort_column.Location = new System.Drawing.Point(87, 564);
            this.cmb_sort_column.Name = "cmb_sort_column";
            this.cmb_sort_column.Size = new System.Drawing.Size(177, 21);
            this.cmb_sort_column.TabIndex = 5;
            // 
            // label_sort_dir
            // 
            this.label_sort_dir.AutoSize = true;
            this.label_sort_dir.Location = new System.Drawing.Point(8, 591);
            this.label_sort_dir.Name = "label_sort_dir";
            this.label_sort_dir.Size = new System.Drawing.Size(71, 13);
            this.label_sort_dir.TabIndex = 3;
            this.label_sort_dir.Text = "Sort Direction";
            // 
            // cmb_sort_dir
            // 
            this.cmb_sort_dir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_sort_dir.FormattingEnabled = true;
            this.cmb_sort_dir.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.cmb_sort_dir.Location = new System.Drawing.Point(87, 588);
            this.cmb_sort_dir.Name = "cmb_sort_dir";
            this.cmb_sort_dir.Size = new System.Drawing.Size(177, 21);
            this.cmb_sort_dir.TabIndex = 5;
            // 
            // syntaxEditor2
            // 
            this.syntaxEditor2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor2.Document = document2;
            this.syntaxEditor2.Font = new System.Drawing.Font("Courier New", 9F);
            this.syntaxEditor2.LineNumberMarginVisible = true;
            this.syntaxEditor2.Location = new System.Drawing.Point(3, 3);
            this.syntaxEditor2.Name = "syntaxEditor2";
            this.syntaxEditor2.Size = new System.Drawing.Size(362, 235);
            this.syntaxEditor2.TabIndex = 11;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(376, 331);
            this.propertyGrid1.TabIndex = 12;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // treeView_computed
            // 
            this.treeView_computed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_computed.FullRowSelect = true;
            this.treeView_computed.HideSelection = false;
            this.treeView_computed.Location = new System.Drawing.Point(3, 28);
            this.treeView_computed.Name = "treeView_computed";
            this.treeView_computed.Size = new System.Drawing.Size(324, 495);
            this.treeView_computed.TabIndex = 7;
            this.treeView_computed.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_computed_NodeMouseClick);
            // 
            // label_current_table
            // 
            this.label_current_table.AutoSize = true;
            this.label_current_table.Location = new System.Drawing.Point(10, 29);
            this.label_current_table.Name = "label_current_table";
            this.label_current_table.Size = new System.Drawing.Size(71, 13);
            this.label_current_table.TabIndex = 1;
            this.label_current_table.Text = "Current Table";
            // 
            // treeView_current
            // 
            this.treeView_current.FullRowSelect = true;
            this.treeView_current.HideSelection = false;
            this.treeView_current.ImageKey = "column";
            this.treeView_current.ImageList = this.imageList1;
            this.treeView_current.Location = new System.Drawing.Point(10, 45);
            this.treeView_current.Name = "treeView_current";
            this.treeView_current.SelectedImageIndex = 0;
            this.treeView_current.Size = new System.Drawing.Size(254, 82);
            this.treeView_current.TabIndex = 4;
            // 
            // btn_add_current
            // 
            this.btn_add_current.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_add_current.Image = ((System.Drawing.Image)(resources.GetObject("btn_add_current.Image")));
            this.btn_add_current.Location = new System.Drawing.Point(272, 72);
            this.btn_add_current.Name = "btn_add_current";
            this.btn_add_current.Size = new System.Drawing.Size(33, 32);
            this.btn_add_current.TabIndex = 6;
            this.btn_add_current.UseVisualStyleBackColor = true;
            this.btn_add_current.Click += new System.EventHandler(this.btn_add_current_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 34);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1850, 630);
            this.splitContainer1.SplitterDistance = 1470;
            this.splitContainer1.TabIndex = 15;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.tabControl2);
            this.splitContainer3.Panel1.Controls.Add(this.label_current_table);
            this.splitContainer3.Panel1.Controls.Add(this.treeView_current);
            this.splitContainer3.Panel1.Controls.Add(this.btn_add_current);
            this.splitContainer3.Panel1.Controls.Add(this.label_parent_tables);
            this.splitContainer3.Panel1.Controls.Add(this.treeView_parents);
            this.splitContainer3.Panel1.Controls.Add(this.btn_add_parent);
            this.splitContainer3.Panel1.Controls.Add(this.label_sub_tables);
            this.splitContainer3.Panel1.Controls.Add(this.treeView_sub);
            this.splitContainer3.Panel1.Controls.Add(this.btn_add_sub);
            this.splitContainer3.Panel1.Controls.Add(this.label_aggregation);
            this.splitContainer3.Panel1.Controls.Add(this.cmb_aggregation);
            this.splitContainer3.Panel1.Controls.Add(this.label_sort_dir);
            this.splitContainer3.Panel1.Controls.Add(this.cmb_sort_dir);
            this.splitContainer3.Panel1.Controls.Add(this.label_sort);
            this.splitContainer3.Panel1.Controls.Add(this.cmb_sort_column);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer_center);
            this.splitContainer3.Size = new System.Drawing.Size(1470, 630);
            this.splitContainer3.SplitterDistance = 669;
            this.splitContainer3.TabIndex = 0;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage_added_columns);
            this.tabControl2.Controls.Add(this.tabPage_computed_columns);
            this.tabControl2.Location = new System.Drawing.Point(320, 22);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(338, 587);
            this.tabControl2.TabIndex = 10;
            // 
            // tabPage_added_columns
            // 
            this.tabPage_added_columns.Controls.Add(this.treeView_added);
            this.tabPage_added_columns.Controls.Add(this.toolStrip3);
            this.tabPage_added_columns.Controls.Add(this.panel_bottom_added_columns);
            this.tabPage_added_columns.Location = new System.Drawing.Point(4, 22);
            this.tabPage_added_columns.Name = "tabPage_added_columns";
            this.tabPage_added_columns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_added_columns.Size = new System.Drawing.Size(330, 561);
            this.tabPage_added_columns.TabIndex = 0;
            this.tabPage_added_columns.Text = "Added Columns";
            this.tabPage_added_columns.UseVisualStyleBackColor = true;
            // 
            // treeView_added
            // 
            this.treeView_added.CheckBoxes = true;
            this.treeView_added.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_added.FullRowSelect = true;
            this.treeView_added.HideSelection = false;
            this.treeView_added.ImageKey = "column";
            this.treeView_added.ImageList = this.imageList1;
            this.treeView_added.Location = new System.Drawing.Point(3, 28);
            this.treeView_added.Name = "treeView_added";
            this.treeView_added.SelectedImageIndex = 0;
            this.treeView_added.Size = new System.Drawing.Size(324, 487);
            this.treeView_added.TabIndex = 8;
            this.treeView_added.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_added_AfterCheck);
            this.treeView_added.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_added_NodeMouseClick);
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_added_new_computation,
            this.toolStripButton_added_new_question,
            this.toolStripButton_col_delete});
            this.toolStrip3.Location = new System.Drawing.Point(3, 3);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(324, 25);
            this.toolStrip3.TabIndex = 9;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // toolStripButton_added_new_computation
            // 
            this.toolStripButton_added_new_computation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_added_new_computation.Image")));
            this.toolStripButton_added_new_computation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_added_new_computation.Name = "toolStripButton_added_new_computation";
            this.toolStripButton_added_new_computation.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton_added_new_computation.Text = "New Computation";
            this.toolStripButton_added_new_computation.ToolTipText = "New Computation from the selected column";
            this.toolStripButton_added_new_computation.Click += new System.EventHandler(this.toolStripButton_added_new_computation_Click);
            // 
            // toolStripButton_added_new_question
            // 
            this.toolStripButton_added_new_question.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_added_new_question.Image")));
            this.toolStripButton_added_new_question.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_added_new_question.Name = "toolStripButton_added_new_question";
            this.toolStripButton_added_new_question.Size = new System.Drawing.Size(102, 22);
            this.toolStripButton_added_new_question.Text = "New Question";
            this.toolStripButton_added_new_question.ToolTipText = "New Question from selected column";
            this.toolStripButton_added_new_question.Click += new System.EventHandler(this.toolStripButton_added_new_question_Click);
            // 
            // toolStripButton_col_delete
            // 
            this.toolStripButton_col_delete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_col_delete.Image")));
            this.toolStripButton_col_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_col_delete.Name = "toolStripButton_col_delete";
            this.toolStripButton_col_delete.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_col_delete.Text = "Delete";
            this.toolStripButton_col_delete.ToolTipText = "Delete selected column";
            this.toolStripButton_col_delete.Click += new System.EventHandler(this.toolStripButton_col_delete_Click);
            // 
            // panel_bottom_added_columns
            // 
            this.panel_bottom_added_columns.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom_added_columns.Location = new System.Drawing.Point(3, 515);
            this.panel_bottom_added_columns.Name = "panel_bottom_added_columns";
            this.panel_bottom_added_columns.Size = new System.Drawing.Size(324, 43);
            this.panel_bottom_added_columns.TabIndex = 1;
            // 
            // tabPage_computed_columns
            // 
            this.tabPage_computed_columns.Controls.Add(this.treeView_computed);
            this.tabPage_computed_columns.Controls.Add(this.toolStrip4);
            this.tabPage_computed_columns.Controls.Add(this.panel_computed_bottom);
            this.tabPage_computed_columns.Location = new System.Drawing.Point(4, 22);
            this.tabPage_computed_columns.Name = "tabPage_computed_columns";
            this.tabPage_computed_columns.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_computed_columns.Size = new System.Drawing.Size(330, 561);
            this.tabPage_computed_columns.TabIndex = 1;
            this.tabPage_computed_columns.Text = "Computed Columns";
            this.tabPage_computed_columns.UseVisualStyleBackColor = true;
            // 
            // toolStrip4
            // 
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_computed,
            this.toolStripButton_computed_delete});
            this.toolStrip4.Location = new System.Drawing.Point(3, 3);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(324, 25);
            this.toolStrip4.TabIndex = 8;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // toolStripButton_new_computed
            // 
            this.toolStripButton_new_computed.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_computed.Image")));
            this.toolStripButton_new_computed.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_computed.Name = "toolStripButton_new_computed";
            this.toolStripButton_new_computed.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_computed.Text = "New";
            this.toolStripButton_new_computed.Click += new System.EventHandler(this.toolStripButton_new_computed_Click);
            // 
            // toolStripButton_computed_delete
            // 
            this.toolStripButton_computed_delete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_computed_delete.Image")));
            this.toolStripButton_computed_delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_computed_delete.Name = "toolStripButton_computed_delete";
            this.toolStripButton_computed_delete.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_computed_delete.Text = "Delete";
            this.toolStripButton_computed_delete.Click += new System.EventHandler(this.toolStripButton_computed_delete_Click);
            // 
            // panel_computed_bottom
            // 
            this.panel_computed_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_computed_bottom.Location = new System.Drawing.Point(3, 523);
            this.panel_computed_bottom.Name = "panel_computed_bottom";
            this.panel_computed_bottom.Size = new System.Drawing.Size(324, 35);
            this.panel_computed_bottom.TabIndex = 1;
            // 
            // splitContainer_center
            // 
            this.splitContainer_center.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_center.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_center.Name = "splitContainer_center";
            this.splitContainer_center.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_center.Panel1
            // 
            this.splitContainer_center.Panel1.Controls.Add(this.splitContainer4);
            // 
            // splitContainer_center.Panel2
            // 
            this.splitContainer_center.Panel2.Controls.Add(this.tabControl3);
            this.splitContainer_center.Panel2.Controls.Add(this.panel5);
            this.splitContainer_center.Size = new System.Drawing.Size(797, 630);
            this.splitContainer_center.SplitterDistance = 377;
            this.splitContainer_center.TabIndex = 1;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.listBox_questions);
            this.splitContainer4.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer4.Panel1.Controls.Add(this.panel4);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.treeView_policies);
            this.splitContainer4.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer4.Panel2.Controls.Add(this.syntaxEditor3);
            this.splitContainer4.Panel2.Controls.Add(this.panel3);
            this.splitContainer4.Size = new System.Drawing.Size(797, 377);
            this.splitContainer4.SplitterDistance = 264;
            this.splitContainer4.TabIndex = 4;
            // 
            // listBox_questions
            // 
            this.listBox_questions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_questions.FormattingEnabled = true;
            this.listBox_questions.Location = new System.Drawing.Point(0, 51);
            this.listBox_questions.Name = "listBox_questions";
            this.listBox_questions.Size = new System.Drawing.Size(264, 326);
            this.listBox_questions.TabIndex = 2;
            this.listBox_questions.SelectedIndexChanged += new System.EventHandler(this.listBox_questions_SelectedIndexChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_question,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 26);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(264, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_new_question
            // 
            this.toolStripButton_new_question.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_question.Image")));
            this.toolStripButton_new_question.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_question.Name = "toolStripButton_new_question";
            this.toolStripButton_new_question.Size = new System.Drawing.Size(102, 22);
            this.toolStripButton_new_question.Text = "New Question";
            this.toolStripButton_new_question.Click += new System.EventHandler(this.toolStripButton_new_question_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton2.Text = "Delete";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label_sec_questions);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(264, 26);
            this.panel4.TabIndex = 3;
            // 
            // label_sec_questions
            // 
            this.label_sec_questions.AutoSize = true;
            this.label_sec_questions.Location = new System.Drawing.Point(4, 8);
            this.label_sec_questions.Name = "label_sec_questions";
            this.label_sec_questions.Size = new System.Drawing.Size(95, 13);
            this.label_sec_questions.TabIndex = 0;
            this.label_sec_questions.Text = "Security Questions";
            // 
            // treeView_policies
            // 
            this.treeView_policies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_policies.FullRowSelect = true;
            this.treeView_policies.HideSelection = false;
            this.treeView_policies.Location = new System.Drawing.Point(0, 51);
            this.treeView_policies.Name = "treeView_policies";
            this.treeView_policies.Size = new System.Drawing.Size(529, 191);
            this.treeView_policies.TabIndex = 0;
            this.treeView_policies.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_select_NodeMouseClick);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_policy,
            this.toolStripButton_new_node,
            this.toolStripSeparator2,
            this.toolStripButton_up,
            this.toolStripButton_down,
            this.toolStripButton_orize,
            this.toolStripButton_andize,
            this.toolStripSeparator1,
            this.toolStripButton_delete_node,
            this.toolStripSeparator3,
            this.toolStripButton_policy_definition});
            this.toolStrip2.Location = new System.Drawing.Point(0, 26);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(529, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripButton_new_policy
            // 
            this.toolStripButton_new_policy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_new_policy.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_policy.Image")));
            this.toolStripButton_new_policy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_policy.Name = "toolStripButton_new_policy";
            this.toolStripButton_new_policy.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton_new_policy.Text = "New Policy";
            this.toolStripButton_new_policy.Click += new System.EventHandler(this.toolStripButton_new_policy_Click);
            // 
            // toolStripButton_new_node
            // 
            this.toolStripButton_new_node.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_node.Image")));
            this.toolStripButton_new_node.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_node.Name = "toolStripButton_new_node";
            this.toolStripButton_new_node.Size = new System.Drawing.Size(100, 22);
            this.toolStripButton_new_node.Text = "Add Question";
            this.toolStripButton_new_node.Click += new System.EventHandler(this.toolStripButton_new_node_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_up
            // 
            this.toolStripButton_up.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_up.Image")));
            this.toolStripButton_up.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_up.Name = "toolStripButton_up";
            this.toolStripButton_up.Size = new System.Drawing.Size(42, 22);
            this.toolStripButton_up.Text = "Up";
            this.toolStripButton_up.Click += new System.EventHandler(this.toolStripButton_up_Click);
            // 
            // toolStripButton_down
            // 
            this.toolStripButton_down.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_down.Image")));
            this.toolStripButton_down.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_down.Name = "toolStripButton_down";
            this.toolStripButton_down.Size = new System.Drawing.Size(58, 22);
            this.toolStripButton_down.Text = "Down";
            this.toolStripButton_down.Click += new System.EventHandler(this.toolStripButton_down_Click);
            // 
            // toolStripButton_orize
            // 
            this.toolStripButton_orize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_orize.Image")));
            this.toolStripButton_orize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_orize.Name = "toolStripButton_orize";
            this.toolStripButton_orize.Size = new System.Drawing.Size(43, 22);
            this.toolStripButton_orize.Text = "OR";
            this.toolStripButton_orize.Click += new System.EventHandler(this.toolStripButton_orize_Click);
            // 
            // toolStripButton_andize
            // 
            this.toolStripButton_andize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_andize.Image")));
            this.toolStripButton_andize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_andize.Name = "toolStripButton_andize";
            this.toolStripButton_andize.Size = new System.Drawing.Size(52, 22);
            this.toolStripButton_andize.Text = "AND";
            this.toolStripButton_andize.Click += new System.EventHandler(this.toolStripButton_andize_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_delete_node
            // 
            this.toolStripButton_delete_node.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_node.Image")));
            this.toolStripButton_delete_node.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_node.Name = "toolStripButton_delete_node";
            this.toolStripButton_delete_node.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_node.Text = "Delete";
            this.toolStripButton_delete_node.Click += new System.EventHandler(this.toolStripButton_delete_node_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton_policy_definition
            // 
            this.toolStripButton_policy_definition.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_policy_definition.Image")));
            this.toolStripButton_policy_definition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_policy_definition.Name = "toolStripButton_policy_definition";
            this.toolStripButton_policy_definition.Size = new System.Drawing.Size(68, 22);
            this.toolStripButton_policy_definition.Text = "Preview";
            this.toolStripButton_policy_definition.Click += new System.EventHandler(this.toolStripButton_policy_definition_Click);
            // 
            // syntaxEditor3
            // 
            this.syntaxEditor3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.syntaxEditor3.Document = document3;
            this.syntaxEditor3.Font = new System.Drawing.Font("Courier New", 8F);
            this.syntaxEditor3.LineNumberMarginVisible = true;
            this.syntaxEditor3.Location = new System.Drawing.Point(0, 242);
            this.syntaxEditor3.Name = "syntaxEditor3";
            this.syntaxEditor3.Size = new System.Drawing.Size(529, 135);
            this.syntaxEditor3.TabIndex = 11;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label_policies);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(529, 26);
            this.panel3.TabIndex = 0;
            // 
            // label_policies
            // 
            this.label_policies.AutoSize = true;
            this.label_policies.Location = new System.Drawing.Point(3, 8);
            this.label_policies.Name = "label_policies";
            this.label_policies.Size = new System.Drawing.Size(84, 13);
            this.label_policies.TabIndex = 0;
            this.label_policies.Text = "Security Policies";
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tab_general);
            this.tabControl3.Controls.Add(this.tab_operations);
            this.tabControl3.Controls.Add(this.tab_validations);
            this.tabControl3.Controls.Add(this.tab_commands);
            this.tabControl3.Controls.Add(this.tab_actions);
            this.tabControl3.Controls.Add(this.tab_workflow);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(0, 26);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(797, 223);
            this.tabControl3.TabIndex = 0;
            // 
            // tab_general
            // 
            this.tab_general.Controls.Add(this.propertyGrid2);
            this.tab_general.Location = new System.Drawing.Point(4, 22);
            this.tab_general.Name = "tab_general";
            this.tab_general.Size = new System.Drawing.Size(789, 197);
            this.tab_general.TabIndex = 5;
            this.tab_general.Text = "Raw";
            this.tab_general.UseVisualStyleBackColor = true;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid2.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(789, 197);
            this.propertyGrid2.TabIndex = 12;
            // 
            // tab_operations
            // 
            this.tab_operations.Controls.Add(this.toolStrip7);
            this.tab_operations.Controls.Add(this.label2);
            this.tab_operations.Controls.Add(this.comboBox_select_policy);
            this.tab_operations.Controls.Add(this.label3);
            this.tab_operations.Controls.Add(this.comboBox_delete_policy);
            this.tab_operations.Controls.Add(this.label4);
            this.tab_operations.Controls.Add(this.comboBox_update_policy);
            this.tab_operations.Location = new System.Drawing.Point(4, 22);
            this.tab_operations.Name = "tab_operations";
            this.tab_operations.Size = new System.Drawing.Size(789, 197);
            this.tab_operations.TabIndex = 0;
            this.tab_operations.Text = "Operations Policies";
            this.tab_operations.UseVisualStyleBackColor = true;
            // 
            // toolStrip7
            // 
            this.toolStrip7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_refresh_operation_policies});
            this.toolStrip7.Location = new System.Drawing.Point(0, 0);
            this.toolStrip7.Name = "toolStrip7";
            this.toolStrip7.Size = new System.Drawing.Size(789, 25);
            this.toolStrip7.TabIndex = 12;
            this.toolStrip7.Text = "toolStrip7";
            // 
            // toolStripButton_refresh_operation_policies
            // 
            this.toolStripButton_refresh_operation_policies.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_refresh_operation_policies.Image")));
            this.toolStripButton_refresh_operation_policies.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_refresh_operation_policies.Name = "toolStripButton_refresh_operation_policies";
            this.toolStripButton_refresh_operation_policies.Size = new System.Drawing.Size(66, 22);
            this.toolStripButton_refresh_operation_policies.Text = "Refresh";
            this.toolStripButton_refresh_operation_policies.Click += new System.EventHandler(this.toolStripButton_refresh_operation_policies_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select";
            // 
            // comboBox_select_policy
            // 
            this.comboBox_select_policy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_select_policy.FormattingEnabled = true;
            this.comboBox_select_policy.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.comboBox_select_policy.Location = new System.Drawing.Point(91, 31);
            this.comboBox_select_policy.Name = "comboBox_select_policy";
            this.comboBox_select_policy.Size = new System.Drawing.Size(177, 21);
            this.comboBox_select_policy.TabIndex = 9;
            this.comboBox_select_policy.SelectedIndexChanged += new System.EventHandler(this.comboBox_select_policy_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Delete";
            // 
            // comboBox_delete_policy
            // 
            this.comboBox_delete_policy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_delete_policy.FormattingEnabled = true;
            this.comboBox_delete_policy.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.comboBox_delete_policy.Location = new System.Drawing.Point(91, 79);
            this.comboBox_delete_policy.Name = "comboBox_delete_policy";
            this.comboBox_delete_policy.Size = new System.Drawing.Size(177, 21);
            this.comboBox_delete_policy.TabIndex = 10;
            this.comboBox_delete_policy.SelectedIndexChanged += new System.EventHandler(this.comboBox_delete_policy_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Update";
            // 
            // comboBox_update_policy
            // 
            this.comboBox_update_policy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_update_policy.FormattingEnabled = true;
            this.comboBox_update_policy.Items.AddRange(new object[] {
            "SUM",
            "COUNT",
            "MAX",
            "MIN",
            "AVG"});
            this.comboBox_update_policy.Location = new System.Drawing.Point(91, 55);
            this.comboBox_update_policy.Name = "comboBox_update_policy";
            this.comboBox_update_policy.Size = new System.Drawing.Size(177, 21);
            this.comboBox_update_policy.TabIndex = 11;
            this.comboBox_update_policy.SelectedIndexChanged += new System.EventHandler(this.comboBox_update_policy_SelectedIndexChanged);
            // 
            // tab_validations
            // 
            this.tab_validations.Controls.Add(this.dataGridView_validators);
            this.tab_validations.Controls.Add(this.toolStrip6);
            this.tab_validations.Location = new System.Drawing.Point(4, 22);
            this.tab_validations.Name = "tab_validations";
            this.tab_validations.Size = new System.Drawing.Size(789, 197);
            this.tab_validations.TabIndex = 1;
            this.tab_validations.Text = "Validations";
            this.tab_validations.UseVisualStyleBackColor = true;
            // 
            // dataGridView_validators
            // 
            this.dataGridView_validators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_validators.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_validators.Location = new System.Drawing.Point(0, 25);
            this.dataGridView_validators.Name = "dataGridView_validators";
            this.dataGridView_validators.Size = new System.Drawing.Size(789, 172);
            this.dataGridView_validators.TabIndex = 0;
            // 
            // toolStrip6
            // 
            this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_validation,
            this.toolStripButton_delete_validation});
            this.toolStrip6.Location = new System.Drawing.Point(0, 0);
            this.toolStrip6.Name = "toolStrip6";
            this.toolStrip6.Size = new System.Drawing.Size(789, 25);
            this.toolStrip6.TabIndex = 1;
            this.toolStrip6.Text = "toolStrip6";
            // 
            // toolStripButton_new_validation
            // 
            this.toolStripButton_new_validation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_validation.Image")));
            this.toolStripButton_new_validation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_validation.Name = "toolStripButton_new_validation";
            this.toolStripButton_new_validation.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_validation.Text = "New";
            // 
            // toolStripButton_delete_validation
            // 
            this.toolStripButton_delete_validation.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_validation.Image")));
            this.toolStripButton_delete_validation.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_validation.Name = "toolStripButton_delete_validation";
            this.toolStripButton_delete_validation.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_validation.Text = "Delete";
            // 
            // tab_commands
            // 
            this.tab_commands.Controls.Add(this.tabControl4);
            this.tab_commands.Location = new System.Drawing.Point(4, 22);
            this.tab_commands.Name = "tab_commands";
            this.tab_commands.Size = new System.Drawing.Size(789, 197);
            this.tab_commands.TabIndex = 4;
            this.tab_commands.Text = "Custom Operations";
            this.tab_commands.UseVisualStyleBackColor = true;
            // 
            // tabControl4
            // 
            this.tabControl4.Controls.Add(this.tab_insert_commands);
            this.tabControl4.Controls.Add(this.tab_update_commands);
            this.tabControl4.Controls.Add(this.tab_delete_commands);
            this.tabControl4.Controls.Add(this.tab_basic_procedures);
            this.tabControl4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl4.Location = new System.Drawing.Point(0, 0);
            this.tabControl4.Name = "tabControl4";
            this.tabControl4.SelectedIndex = 0;
            this.tabControl4.Size = new System.Drawing.Size(789, 197);
            this.tabControl4.TabIndex = 2;
            // 
            // tab_insert_commands
            // 
            this.tab_insert_commands.Controls.Add(this.listBox_insert_commands);
            this.tab_insert_commands.Controls.Add(this.toolStrip9);
            this.tab_insert_commands.Location = new System.Drawing.Point(4, 22);
            this.tab_insert_commands.Name = "tab_insert_commands";
            this.tab_insert_commands.Padding = new System.Windows.Forms.Padding(3);
            this.tab_insert_commands.Size = new System.Drawing.Size(781, 171);
            this.tab_insert_commands.TabIndex = 0;
            this.tab_insert_commands.Text = "Insert";
            this.tab_insert_commands.UseVisualStyleBackColor = true;
            // 
            // listBox_insert_commands
            // 
            this.listBox_insert_commands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_insert_commands.FormattingEnabled = true;
            this.listBox_insert_commands.Location = new System.Drawing.Point(3, 28);
            this.listBox_insert_commands.Name = "listBox_insert_commands";
            this.listBox_insert_commands.Size = new System.Drawing.Size(775, 140);
            this.listBox_insert_commands.TabIndex = 3;
            this.listBox_insert_commands.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox_commands_MouseClick);
            // 
            // toolStrip9
            // 
            this.toolStrip9.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_insert_command,
            this.toolStripButton_delete_insert_command});
            this.toolStrip9.Location = new System.Drawing.Point(3, 3);
            this.toolStrip9.Name = "toolStrip9";
            this.toolStrip9.Size = new System.Drawing.Size(775, 25);
            this.toolStrip9.TabIndex = 2;
            this.toolStrip9.Text = "toolStrip9";
            // 
            // toolStripButton_new_insert_command
            // 
            this.toolStripButton_new_insert_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_insert_command.Image")));
            this.toolStripButton_new_insert_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_insert_command.Name = "toolStripButton_new_insert_command";
            this.toolStripButton_new_insert_command.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_insert_command.Text = "New";
            this.toolStripButton_new_insert_command.Click += new System.EventHandler(this.toolStripButton_new_insert_command_Click);
            // 
            // toolStripButton_delete_insert_command
            // 
            this.toolStripButton_delete_insert_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_insert_command.Image")));
            this.toolStripButton_delete_insert_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_insert_command.Name = "toolStripButton_delete_insert_command";
            this.toolStripButton_delete_insert_command.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_insert_command.Text = "Delete";
            // 
            // tab_update_commands
            // 
            this.tab_update_commands.Controls.Add(this.listBox_update_commands);
            this.tab_update_commands.Controls.Add(this.toolStrip8);
            this.tab_update_commands.Location = new System.Drawing.Point(4, 22);
            this.tab_update_commands.Name = "tab_update_commands";
            this.tab_update_commands.Padding = new System.Windows.Forms.Padding(3);
            this.tab_update_commands.Size = new System.Drawing.Size(781, 171);
            this.tab_update_commands.TabIndex = 1;
            this.tab_update_commands.Text = "Update";
            this.tab_update_commands.UseVisualStyleBackColor = true;
            // 
            // listBox_update_commands
            // 
            this.listBox_update_commands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_update_commands.FormattingEnabled = true;
            this.listBox_update_commands.Location = new System.Drawing.Point(3, 28);
            this.listBox_update_commands.Name = "listBox_update_commands";
            this.listBox_update_commands.Size = new System.Drawing.Size(775, 140);
            this.listBox_update_commands.TabIndex = 2;
            this.listBox_update_commands.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox_commands_MouseClick);
            // 
            // toolStrip8
            // 
            this.toolStrip8.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_update_command,
            this.toolStripButton_delete_update_command});
            this.toolStrip8.Location = new System.Drawing.Point(3, 3);
            this.toolStrip8.Name = "toolStrip8";
            this.toolStrip8.Size = new System.Drawing.Size(775, 25);
            this.toolStrip8.TabIndex = 1;
            this.toolStrip8.Text = "toolStrip8";
            // 
            // toolStripButton_new_update_command
            // 
            this.toolStripButton_new_update_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_update_command.Image")));
            this.toolStripButton_new_update_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_update_command.Name = "toolStripButton_new_update_command";
            this.toolStripButton_new_update_command.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_update_command.Text = "New";
            this.toolStripButton_new_update_command.Click += new System.EventHandler(this.toolStripButton_new_update_command_Click);
            // 
            // toolStripButton_delete_update_command
            // 
            this.toolStripButton_delete_update_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_update_command.Image")));
            this.toolStripButton_delete_update_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_update_command.Name = "toolStripButton_delete_update_command";
            this.toolStripButton_delete_update_command.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_update_command.Text = "Delete";
            this.toolStripButton_delete_update_command.Click += new System.EventHandler(this.toolStripButton_delete_update_command_Click);
            // 
            // tab_delete_commands
            // 
            this.tab_delete_commands.Controls.Add(this.listBox_delete_commands);
            this.tab_delete_commands.Controls.Add(this.toolStrip10);
            this.tab_delete_commands.Location = new System.Drawing.Point(4, 22);
            this.tab_delete_commands.Name = "tab_delete_commands";
            this.tab_delete_commands.Size = new System.Drawing.Size(781, 171);
            this.tab_delete_commands.TabIndex = 2;
            this.tab_delete_commands.Text = "Delete";
            this.tab_delete_commands.UseVisualStyleBackColor = true;
            // 
            // listBox_delete_commands
            // 
            this.listBox_delete_commands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_delete_commands.FormattingEnabled = true;
            this.listBox_delete_commands.Location = new System.Drawing.Point(0, 25);
            this.listBox_delete_commands.Name = "listBox_delete_commands";
            this.listBox_delete_commands.Size = new System.Drawing.Size(781, 146);
            this.listBox_delete_commands.TabIndex = 3;
            this.listBox_delete_commands.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox_commands_MouseClick);
            // 
            // toolStrip10
            // 
            this.toolStrip10.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_delete_command,
            this.toolStripButton_delete_delete_command});
            this.toolStrip10.Location = new System.Drawing.Point(0, 0);
            this.toolStrip10.Name = "toolStrip10";
            this.toolStrip10.Size = new System.Drawing.Size(781, 25);
            this.toolStrip10.TabIndex = 2;
            this.toolStrip10.Text = "toolStrip10";
            // 
            // toolStripButton_new_delete_command
            // 
            this.toolStripButton_new_delete_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_delete_command.Image")));
            this.toolStripButton_new_delete_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_delete_command.Name = "toolStripButton_new_delete_command";
            this.toolStripButton_new_delete_command.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_delete_command.Text = "New";
            // 
            // toolStripButton_delete_delete_command
            // 
            this.toolStripButton_delete_delete_command.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_delete_command.Image")));
            this.toolStripButton_delete_delete_command.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_delete_command.Name = "toolStripButton_delete_delete_command";
            this.toolStripButton_delete_delete_command.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_delete_command.Text = "Delete";
            // 
            // tab_basic_procedures
            // 
            this.tab_basic_procedures.Controls.Add(this.listBox_basic_procedures);
            this.tab_basic_procedures.Controls.Add(this.toolStrip11);
            this.tab_basic_procedures.Location = new System.Drawing.Point(4, 22);
            this.tab_basic_procedures.Name = "tab_basic_procedures";
            this.tab_basic_procedures.Size = new System.Drawing.Size(781, 171);
            this.tab_basic_procedures.TabIndex = 3;
            this.tab_basic_procedures.Text = "Basic Procedures";
            this.tab_basic_procedures.UseVisualStyleBackColor = true;
            // 
            // listBox_basic_procedures
            // 
            this.listBox_basic_procedures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_basic_procedures.FormattingEnabled = true;
            this.listBox_basic_procedures.Location = new System.Drawing.Point(0, 25);
            this.listBox_basic_procedures.Name = "listBox_basic_procedures";
            this.listBox_basic_procedures.Size = new System.Drawing.Size(781, 146);
            this.listBox_basic_procedures.TabIndex = 4;
            this.listBox_basic_procedures.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listBox_basic_procedures_MouseClick);
            // 
            // toolStrip11
            // 
            this.toolStrip11.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_basic_procedure,
            this.toolStripButton_delete_basic_procedure});
            this.toolStrip11.Location = new System.Drawing.Point(0, 0);
            this.toolStrip11.Name = "toolStrip11";
            this.toolStrip11.Size = new System.Drawing.Size(781, 25);
            this.toolStrip11.TabIndex = 3;
            this.toolStrip11.Text = "toolStrip11";
            // 
            // toolStripButton_new_basic_procedure
            // 
            this.toolStripButton_new_basic_procedure.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_basic_procedure.Image")));
            this.toolStripButton_new_basic_procedure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_basic_procedure.Name = "toolStripButton_new_basic_procedure";
            this.toolStripButton_new_basic_procedure.Size = new System.Drawing.Size(51, 22);
            this.toolStripButton_new_basic_procedure.Text = "New";
            this.toolStripButton_new_basic_procedure.Click += new System.EventHandler(this.toolStripButton_new_basic_procedure_Click);
            // 
            // toolStripButton_delete_basic_procedure
            // 
            this.toolStripButton_delete_basic_procedure.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_delete_basic_procedure.Image")));
            this.toolStripButton_delete_basic_procedure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_delete_basic_procedure.Name = "toolStripButton_delete_basic_procedure";
            this.toolStripButton_delete_basic_procedure.Size = new System.Drawing.Size(60, 22);
            this.toolStripButton_delete_basic_procedure.Text = "Delete";
            // 
            // tab_actions
            // 
            this.tab_actions.Controls.Add(this.flowLayoutPanel_actions);
            this.tab_actions.Controls.Add(this.toolStrip5);
            this.tab_actions.Location = new System.Drawing.Point(4, 22);
            this.tab_actions.Name = "tab_actions";
            this.tab_actions.Size = new System.Drawing.Size(789, 197);
            this.tab_actions.TabIndex = 2;
            this.tab_actions.Text = "Actions";
            this.tab_actions.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel_actions
            // 
            this.flowLayoutPanel_actions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel_actions.Location = new System.Drawing.Point(0, 25);
            this.flowLayoutPanel_actions.Name = "flowLayoutPanel_actions";
            this.flowLayoutPanel_actions.Size = new System.Drawing.Size(789, 172);
            this.flowLayoutPanel_actions.TabIndex = 1;
            // 
            // toolStrip5
            // 
            this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_new_action});
            this.toolStrip5.Location = new System.Drawing.Point(0, 0);
            this.toolStrip5.Name = "toolStrip5";
            this.toolStrip5.Size = new System.Drawing.Size(789, 25);
            this.toolStrip5.TabIndex = 0;
            this.toolStrip5.Text = "toolStrip5";
            // 
            // toolStripButton_new_action
            // 
            this.toolStripButton_new_action.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_new_action.Image")));
            this.toolStripButton_new_action.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_new_action.Name = "toolStripButton_new_action";
            this.toolStripButton_new_action.Size = new System.Drawing.Size(89, 22);
            this.toolStripButton_new_action.Text = "New Action";
            this.toolStripButton_new_action.Click += new System.EventHandler(this.toolStripButton_new_action_Click);
            // 
            // tab_workflow
            // 
            this.tab_workflow.Controls.Add(this.button_manage_workflow);
            this.tab_workflow.Location = new System.Drawing.Point(4, 22);
            this.tab_workflow.Name = "tab_workflow";
            this.tab_workflow.Size = new System.Drawing.Size(789, 197);
            this.tab_workflow.TabIndex = 3;
            this.tab_workflow.Text = "Workflow Transition Actions";
            this.tab_workflow.UseVisualStyleBackColor = true;
            // 
            // button_manage_workflow
            // 
            this.button_manage_workflow.Location = new System.Drawing.Point(24, 27);
            this.button_manage_workflow.Name = "button_manage_workflow";
            this.button_manage_workflow.Size = new System.Drawing.Size(208, 23);
            this.button_manage_workflow.TabIndex = 0;
            this.button_manage_workflow.Text = "Manage Workflow";
            this.button_manage_workflow.UseVisualStyleBackColor = true;
            this.button_manage_workflow.Click += new System.EventHandler(this.button_manage_workflow_Click);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(797, 26);
            this.panel5.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.propertyGrid1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer2.Panel2.Controls.Add(this.panel_syntax_top);
            this.splitContainer2.Size = new System.Drawing.Size(376, 630);
            this.splitContainer2.SplitterDistance = 331;
            this.splitContainer2.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(376, 267);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.syntaxEditor1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(368, 241);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SQL";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.syntaxEditor2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(368, 241);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "JSON";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel_syntax_top
            // 
            this.panel_syntax_top.Controls.Add(this.comboBox_syntax_source);
            this.panel_syntax_top.Controls.Add(this.label_syntax_source);
            this.panel_syntax_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_syntax_top.Location = new System.Drawing.Point(0, 0);
            this.panel_syntax_top.Name = "panel_syntax_top";
            this.panel_syntax_top.Size = new System.Drawing.Size(376, 28);
            this.panel_syntax_top.TabIndex = 13;
            // 
            // comboBox_syntax_source
            // 
            this.comboBox_syntax_source.FormattingEnabled = true;
            this.comboBox_syntax_source.Items.AddRange(new object[] {
            "Extended View",
            "Select Policy",
            "Update Policy",
            "Delete Policy"});
            this.comboBox_syntax_source.Location = new System.Drawing.Point(113, 4);
            this.comboBox_syntax_source.Name = "comboBox_syntax_source";
            this.comboBox_syntax_source.Size = new System.Drawing.Size(150, 21);
            this.comboBox_syntax_source.TabIndex = 1;
            // 
            // label_syntax_source
            // 
            this.label_syntax_source.AutoSize = true;
            this.label_syntax_source.Location = new System.Drawing.Point(7, 7);
            this.label_syntax_source.Name = "label_syntax_source";
            this.label_syntax_source.Size = new System.Drawing.Size(99, 13);
            this.label_syntax_source.TabIndex = 0;
            this.label_syntax_source.Text = "Show Definition for:";
            // 
            // panel_top
            // 
            this.panel_top.Controls.Add(this.lbl_source);
            this.panel_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_top.Location = new System.Drawing.Point(0, 0);
            this.panel_top.Name = "panel_top";
            this.panel_top.Size = new System.Drawing.Size(1850, 34);
            this.panel_top.TabIndex = 0;
            // 
            // panel_bottom
            // 
            this.panel_bottom.Controls.Add(this.dataGridView1);
            this.panel_bottom.Controls.Add(this.label_preview);
            this.panel_bottom.Controls.Add(this.panel1);
            this.panel_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel_bottom.Location = new System.Drawing.Point(0, 664);
            this.panel_bottom.Name = "panel_bottom";
            this.panel_bottom.Size = new System.Drawing.Size(1850, 216);
            this.panel_bottom.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 172);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1850, 44);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btn_apply);
            this.panel2.Controls.Add(this.btn_preview_computed);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1291, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(559, 44);
            this.panel2.TabIndex = 7;
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(290, 9);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(75, 23);
            this.btn_apply.TabIndex = 8;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_preview_computed
            // 
            this.btn_preview_computed.Location = new System.Drawing.Point(144, 9);
            this.btn_preview_computed.Name = "btn_preview_computed";
            this.btn_preview_computed.Size = new System.Drawing.Size(135, 23);
            this.btn_preview_computed.TabIndex = 7;
            this.btn_preview_computed.Text = "Preview Computed View";
            this.btn_preview_computed.UseVisualStyleBackColor = true;
            this.btn_preview_computed.Click += new System.EventHandler(this.btn_preview_computed_Click);
            // 
            // ManageRelatedColumns
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1850, 880);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel_top);
            this.Controls.Add(this.panel_bottom);
            this.Name = "ManageRelatedColumns";
            this.Text = "Manage Related Columns";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.tabPage_added_columns.ResumeLayout(false);
            this.tabPage_added_columns.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.tabPage_computed_columns.ResumeLayout(false);
            this.tabPage_computed_columns.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.splitContainer_center.Panel1.ResumeLayout(false);
            this.splitContainer_center.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_center)).EndInit();
            this.splitContainer_center.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControl3.ResumeLayout(false);
            this.tab_general.ResumeLayout(false);
            this.tab_operations.ResumeLayout(false);
            this.tab_operations.PerformLayout();
            this.toolStrip7.ResumeLayout(false);
            this.toolStrip7.PerformLayout();
            this.tab_validations.ResumeLayout(false);
            this.tab_validations.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_validators)).EndInit();
            this.toolStrip6.ResumeLayout(false);
            this.toolStrip6.PerformLayout();
            this.tab_commands.ResumeLayout(false);
            this.tabControl4.ResumeLayout(false);
            this.tab_insert_commands.ResumeLayout(false);
            this.tab_insert_commands.PerformLayout();
            this.toolStrip9.ResumeLayout(false);
            this.toolStrip9.PerformLayout();
            this.tab_update_commands.ResumeLayout(false);
            this.tab_update_commands.PerformLayout();
            this.toolStrip8.ResumeLayout(false);
            this.toolStrip8.PerformLayout();
            this.tab_delete_commands.ResumeLayout(false);
            this.tab_delete_commands.PerformLayout();
            this.toolStrip10.ResumeLayout(false);
            this.toolStrip10.PerformLayout();
            this.tab_basic_procedures.ResumeLayout(false);
            this.tab_basic_procedures.PerformLayout();
            this.toolStrip11.ResumeLayout(false);
            this.toolStrip11.PerformLayout();
            this.tab_actions.ResumeLayout(false);
            this.tab_actions.PerformLayout();
            this.toolStrip5.ResumeLayout(false);
            this.toolStrip5.PerformLayout();
            this.tab_workflow.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel_syntax_top.ResumeLayout(false);
            this.panel_syntax_top.PerformLayout();
            this.panel_top.ResumeLayout(false);
            this.panel_top.PerformLayout();
            this.panel_bottom.ResumeLayout(false);
            this.panel_bottom.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label_parent_tables;
        private System.Windows.Forms.Label label_sub_tables;
        private System.Windows.Forms.TreeView treeView_parents;
        private System.Windows.Forms.TreeView treeView_sub;
        private System.Windows.Forms.ComboBox cmb_aggregation;
        private System.Windows.Forms.Label label_aggregation;
        private System.Windows.Forms.Button btn_add_parent;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label_preview;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lbl_source;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button btn_add_sub;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor1;
        private System.Windows.Forms.Label label_sort;
        private System.Windows.Forms.ComboBox cmb_sort_column;
        private System.Windows.Forms.Label label_sort_dir;
        private System.Windows.Forms.ComboBox cmb_sort_dir;
        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TreeView treeView_computed;
        private System.Windows.Forms.Label label_current_table;
        private System.Windows.Forms.TreeView treeView_current;
        private System.Windows.Forms.Button btn_add_current;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel panel_bottom_added_columns;
        private System.Windows.Forms.Panel panel_computed_bottom;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel_top;
        private System.Windows.Forms.Panel panel_bottom;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer_center;
        private System.Windows.Forms.TabPage tabPage_added_columns;
        private System.Windows.Forms.TabPage tabPage_computed_columns;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label_policies;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel_syntax_top;
        private System.Windows.Forms.ComboBox comboBox_syntax_source;
        private System.Windows.Forms.Label label_syntax_source;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_question;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.TreeView treeView_policies;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_node;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_node;
        private System.Windows.Forms.ListBox listBox_questions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton_orize;
        private System.Windows.Forms.ToolStripButton toolStripButton_andize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TreeView treeView_added;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label_sec_questions;
        private System.Windows.Forms.ToolStripButton toolStripButton_up;
        private System.Windows.Forms.ToolStripButton toolStripButton_down;
        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor3;
        private System.Windows.Forms.ToolStripButton toolStripButton_policy_definition;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton toolStripButton_added_new_computation;
        private System.Windows.Forms.ToolStripButton toolStripButton_added_new_question;
        private System.Windows.Forms.ToolStripButton toolStripButton_col_delete;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_computed;
        private System.Windows.Forms.ToolStripButton toolStripButton_computed_delete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tab_operations;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox_select_policy;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox_delete_policy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox_update_policy;
        private System.Windows.Forms.TabPage tab_validations;
        private System.Windows.Forms.DataGridView dataGridView_validators;
        private System.Windows.Forms.TabPage tab_actions;
        private System.Windows.Forms.TabPage tab_workflow;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ToolStrip toolStrip5;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_action;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel_actions;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_policy;
        private System.Windows.Forms.ToolStrip toolStrip6;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_validation;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_validation;
        private System.Windows.Forms.Button button_manage_workflow;
        private System.Windows.Forms.ToolStrip toolStrip7;
        private System.Windows.Forms.ToolStripButton toolStripButton_refresh_operation_policies;
        private System.Windows.Forms.TabPage tab_commands;
        private System.Windows.Forms.TabControl tabControl4;
        private System.Windows.Forms.TabPage tab_insert_commands;
        private System.Windows.Forms.ListBox listBox_insert_commands;
        private System.Windows.Forms.ToolStrip toolStrip9;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_insert_command;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_insert_command;
        private System.Windows.Forms.TabPage tab_update_commands;
        private System.Windows.Forms.ListBox listBox_update_commands;
        private System.Windows.Forms.ToolStrip toolStrip8;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_update_command;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_update_command;
        private System.Windows.Forms.TabPage tab_delete_commands;
        private System.Windows.Forms.ListBox listBox_delete_commands;
        private System.Windows.Forms.ToolStrip toolStrip10;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_delete_command;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_delete_command;
        private System.Windows.Forms.TabPage tab_basic_procedures;
        private System.Windows.Forms.ListBox listBox_basic_procedures;
        private System.Windows.Forms.ToolStrip toolStrip11;
        private System.Windows.Forms.ToolStripButton toolStripButton_new_basic_procedure;
        private System.Windows.Forms.ToolStripButton toolStripButton_delete_basic_procedure;
        private System.Windows.Forms.TabPage tab_general;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.Button btn_preview_computed;
        private System.Windows.Forms.Button btn_apply;
    }
}