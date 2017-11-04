namespace EngineManager
{
    partial class DBCommandPreview
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
            ActiproSoftware.SyntaxEditor.Document document2 = new ActiproSoftware.SyntaxEditor.Document();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DBCommandPreview));
            this.syntaxEditor1 = new ActiproSoftware.SyntaxEditor.SyntaxEditor();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_execute = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_reset = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // syntaxEditor1
            // 
            this.syntaxEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.syntaxEditor1.Document = document2;
            this.syntaxEditor1.Location = new System.Drawing.Point(0, 0);
            this.syntaxEditor1.Name = "syntaxEditor1";
            this.syntaxEditor1.Size = new System.Drawing.Size(561, 463);
            this.syntaxEditor1.TabIndex = 0;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.syntaxEditor1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(561, 463);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(561, 488);
            this.toolStripContainer1.TabIndex = 1;
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
            this.toolStripButton_execute,
            this.toolStripButton_reset});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(165, 25);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButton_execute
            // 
            this.toolStripButton_execute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_execute.Image")));
            this.toolStripButton_execute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_execute.Name = "toolStripButton_execute";
            this.toolStripButton_execute.Size = new System.Drawing.Size(67, 22);
            this.toolStripButton_execute.Text = "Execute";
            this.toolStripButton_execute.Click += new System.EventHandler(this.toolStripButton_execute_Click);
            // 
            // toolStripButton_reset
            // 
            this.toolStripButton_reset.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_reset.Image")));
            this.toolStripButton_reset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_reset.Name = "toolStripButton_reset";
            this.toolStripButton_reset.Size = new System.Drawing.Size(55, 22);
            this.toolStripButton_reset.Text = "Reset";
            this.toolStripButton_reset.Click += new System.EventHandler(this.toolStripButton_reset_Click);
            // 
            // DBCommandPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 488);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "DBCommandPreview";
            this.Text = "DBCommandPreview";
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

        private ActiproSoftware.SyntaxEditor.SyntaxEditor syntaxEditor1;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_execute;
        private System.Windows.Forms.ToolStripButton toolStripButton_reset;
    }
}