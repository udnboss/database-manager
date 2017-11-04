using ActiproSoftware.SyntaxEditor;

namespace EngineManager
{
    partial class Migration
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
            ActiproSoftware.SyntaxEditor.Document document7 = new ActiproSoftware.SyntaxEditor.Document();
            ActiproSoftware.SyntaxEditor.Document document8 = new ActiproSoftware.SyntaxEditor.Document();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Migration));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_rules = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tabPage_script = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.syntaxEditor1 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.syntaxEditor2 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.tabPage_progress = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btn_delete_profile = new System.Windows.Forms.Button();
            this.btn_new_profile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_profiles = new System.Windows.Forms.ComboBox();
            this.cmb_target_server = new System.Windows.Forms.ComboBox();
            this.btn_start = new System.Windows.Forms.Button();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmb_structure_sources = new System.Windows.Forms.ToolStripComboBox();
            this.btn_save = new System.Windows.Forms.ToolStripButton();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage_rules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage_script.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.tabPage_progress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(1210, 656);
            this.splitContainer1.SplitterDistance = 746;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_rules);
            this.tabControl1.Controls.Add(this.tabPage_script);
            this.tabControl1.Controls.Add(this.tabPage_progress);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(746, 656);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage_rules
            // 
            this.tabPage_rules.Controls.Add(this.dataGridView1);
            this.tabPage_rules.Location = new System.Drawing.Point(4, 22);
            this.tabPage_rules.Name = "tabPage_rules";
            this.tabPage_rules.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_rules.Size = new System.Drawing.Size(738, 630);
            this.tabPage_rules.TabIndex = 0;
            this.tabPage_rules.Text = "Rules";
            this.tabPage_rules.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(732, 624);
            this.dataGridView1.TabIndex = 0;
            // 
            // tabPage_script
            // 
            this.tabPage_script.Controls.Add(this.splitContainer3);
            this.tabPage_script.Location = new System.Drawing.Point(4, 22);
            this.tabPage_script.Name = "tabPage_script";
            this.tabPage_script.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_script.Size = new System.Drawing.Size(738, 628);
            this.tabPage_script.TabIndex = 1;
            this.tabPage_script.Text = "Script Preview";
            this.tabPage_script.UseVisualStyleBackColor = true;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.syntaxEditor1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.syntaxEditor2);
            this.splitContainer3.Size = new System.Drawing.Size(732, 622);
            this.splitContainer3.SplitterDistance = 243;
            this.splitContainer3.TabIndex = 1;
            // 
            // syntaxEditor1
            // 
            this.syntaxEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor1.Document = document7;
            this.syntaxEditor1.LineNumberMarginVisible = true;
            this.syntaxEditor1.Location = new System.Drawing.Point(0, 0);
            this.syntaxEditor1.Name = "syntaxEditor1";
            this.syntaxEditor1.Size = new System.Drawing.Size(732, 243);
            this.syntaxEditor1.TabIndex = 0;
            // 
            // syntaxEditor2
            // 
            this.syntaxEditor2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor2.Document = document8;
            this.syntaxEditor2.LineNumberMarginVisible = true;
            this.syntaxEditor2.Location = new System.Drawing.Point(0, 0);
            this.syntaxEditor2.Name = "syntaxEditor2";
            this.syntaxEditor2.Size = new System.Drawing.Size(732, 375);
            this.syntaxEditor2.TabIndex = 1;
            // 
            // tabPage_progress
            // 
            this.tabPage_progress.Controls.Add(this.dataGridView2);
            this.tabPage_progress.Location = new System.Drawing.Point(4, 22);
            this.tabPage_progress.Name = "tabPage_progress";
            this.tabPage_progress.Size = new System.Drawing.Size(738, 628);
            this.tabPage_progress.TabIndex = 2;
            this.tabPage_progress.Text = "Progress";
            this.tabPage_progress.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.Size = new System.Drawing.Size(738, 628);
            this.dataGridView2.TabIndex = 0;
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
            this.splitContainer2.Panel1.Controls.Add(this.btn_delete_profile);
            this.splitContainer2.Panel1.Controls.Add(this.btn_new_profile);
            this.splitContainer2.Panel1.Controls.Add(this.label3);
            this.splitContainer2.Panel1.Controls.Add(this.label2);
            this.splitContainer2.Panel1.Controls.Add(this.cmb_profiles);
            this.splitContainer2.Panel1.Controls.Add(this.cmb_target_server);
            this.splitContainer2.Panel1.Controls.Add(this.btn_start);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer2.Size = new System.Drawing.Size(460, 656);
            this.splitContainer2.SplitterDistance = 204;
            this.splitContainer2.TabIndex = 4;
            // 
            // btn_delete_profile
            // 
            this.btn_delete_profile.Location = new System.Drawing.Point(373, 39);
            this.btn_delete_profile.Name = "btn_delete_profile";
            this.btn_delete_profile.Size = new System.Drawing.Size(75, 23);
            this.btn_delete_profile.TabIndex = 6;
            this.btn_delete_profile.Text = "Delete";
            this.btn_delete_profile.UseVisualStyleBackColor = true;
            this.btn_delete_profile.Click += new System.EventHandler(this.btn_delete_profile_Click);
            // 
            // btn_new_profile
            // 
            this.btn_new_profile.Location = new System.Drawing.Point(291, 39);
            this.btn_new_profile.Name = "btn_new_profile";
            this.btn_new_profile.Size = new System.Drawing.Size(75, 23);
            this.btn_new_profile.TabIndex = 5;
            this.btn_new_profile.Text = "New Profile";
            this.btn_new_profile.UseVisualStyleBackColor = true;
            this.btn_new_profile.Click += new System.EventHandler(this.btn_new_profile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Migration Profile";
            // 
            // cmb_profiles
            // 
            this.cmb_profiles.FormattingEnabled = true;
            this.cmb_profiles.Location = new System.Drawing.Point(17, 41);
            this.cmb_profiles.Name = "cmb_profiles";
            this.cmb_profiles.Size = new System.Drawing.Size(242, 21);
            this.cmb_profiles.TabIndex = 3;
            this.cmb_profiles.SelectedIndexChanged += new System.EventHandler(this.cmb_profiles_SelectedIndexChanged);
            // 
            // cmb_target_server
            // 
            this.cmb_target_server.FormattingEnabled = true;
            this.cmb_target_server.Location = new System.Drawing.Point(17, 107);
            this.cmb_target_server.Name = "cmb_target_server";
            this.cmb_target_server.Size = new System.Drawing.Size(242, 21);
            this.cmb_target_server.TabIndex = 1;
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(291, 105);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(157, 23);
            this.btn_start.TabIndex = 2;
            this.btn_start.Text = "Start Migration";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(460, 448);
            this.propertyGrid1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target Server";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1210, 656);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(1210, 681);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmb_structure_sources,
            this.btn_save});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(292, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(89, 22);
            this.toolStripLabel1.Text = "Set Data Source";
            // 
            // cmb_structure_sources
            // 
            this.cmb_structure_sources.Name = "cmb_structure_sources";
            this.cmb_structure_sources.Size = new System.Drawing.Size(121, 25);
            this.cmb_structure_sources.SelectedIndexChanged += new System.EventHandler(this.cmb_structure_sources_SelectedIndexChanged);
            // 
            // btn_save
            // 
            this.btn_save.Image = ((System.Drawing.Image)(resources.GetObject("btn_save.Image")));
            this.btn_save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(68, 22);
            this.btn_save.Text = "Save All";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Migration Target";
            // 
            // Migration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 681);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "Migration";
            this.Text = "Migration";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage_rules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage_script.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.tabPage_progress.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ComboBox cmb_target_server;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmb_structure_sources;
        private System.Windows.Forms.ToolStripButton btn_save;
        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_rules;
        private System.Windows.Forms.TabPage tabPage_script;
        private SyntaxEditor syntaxEditor1;
        private System.Windows.Forms.TabPage tabPage_progress;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private SyntaxEditor syntaxEditor2;
        private System.Windows.Forms.Button btn_new_profile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmb_profiles;
        private System.Windows.Forms.Button btn_delete_profile;
        private System.Windows.Forms.Label label3;
    }
}