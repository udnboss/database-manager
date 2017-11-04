namespace EngineManager
{
    partial class IconClassForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.cb_width = new System.Windows.Forms.CheckBox();
            this.cmb_color = new System.Windows.Forms.ComboBox();
            this.cmb_rotate = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_no_icon = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.ColumnWidth = 300;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 64);
            this.listBox1.MultiColumn = true;
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(983, 381);
            this.listBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(920, 548);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 27);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(165, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyUp);
            // 
            // cb_width
            // 
            this.cb_width.AutoSize = true;
            this.cb_width.Location = new System.Drawing.Point(12, 467);
            this.cb_width.Name = "cb_width";
            this.cb_width.Size = new System.Drawing.Size(82, 17);
            this.cb_width.TabIndex = 3;
            this.cb_width.Text = "Fixed Width";
            this.cb_width.UseVisualStyleBackColor = true;
            // 
            // cmb_color
            // 
            this.cmb_color.FormattingEnabled = true;
            this.cmb_color.Items.AddRange(new object[] {
            "",
            "text-default",
            "text-info",
            "text-warning",
            "text-success",
            "text-primary",
            "text-danger"});
            this.cmb_color.Location = new System.Drawing.Point(200, 467);
            this.cmb_color.Name = "cmb_color";
            this.cmb_color.Size = new System.Drawing.Size(121, 21);
            this.cmb_color.TabIndex = 4;
            // 
            // cmb_rotate
            // 
            this.cmb_rotate.FormattingEnabled = true;
            this.cmb_rotate.Items.AddRange(new object[] {
            "",
            "fa-rotate-90",
            "fa-rotate-180",
            "fa-rotate-270",
            "fa-flip-horizontal",
            "fa-flip-vertical"});
            this.cmb_rotate.Location = new System.Drawing.Point(406, 467);
            this.cmb_rotate.Name = "cmb_rotate";
            this.cmb_rotate.Size = new System.Drawing.Size(121, 21);
            this.cmb_rotate.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 452);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(406, 452);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Rotate";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Search";
            // 
            // btn_no_icon
            // 
            this.btn_no_icon.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_no_icon.Location = new System.Drawing.Point(828, 548);
            this.btn_no_icon.Name = "btn_no_icon";
            this.btn_no_icon.Size = new System.Drawing.Size(75, 23);
            this.btn_no_icon.TabIndex = 9;
            this.btn_no_icon.Text = "No Icon";
            this.btn_no_icon.UseVisualStyleBackColor = true;
            this.btn_no_icon.Click += new System.EventHandler(this.btn_no_icon_Click);
            // 
            // IconClassForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 583);
            this.Controls.Add(this.btn_no_icon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmb_rotate);
            this.Controls.Add(this.cmb_color);
            this.Controls.Add(this.cb_width);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Name = "IconClassForm";
            this.Text = "IconClassForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox cb_width;
        private System.Windows.Forms.ComboBox cmb_color;
        private System.Windows.Forms.ComboBox cmb_rotate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_no_icon;
    }
}