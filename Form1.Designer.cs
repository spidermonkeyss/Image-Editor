namespace ImageEditor
{
    partial class Form1
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pickColorBtn = new System.Windows.Forms.Button();
            this.colorDisplayTextBox = new System.Windows.Forms.TextBox();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.toolGroupBox = new System.Windows.Forms.GroupBox();
            this.bucketRadioButton = new System.Windows.Forms.RadioButton();
            this.boxRadioButton = new System.Windows.Forms.RadioButton();
            this.drawRadioButton = new System.Windows.Forms.RadioButton();
            this.eraseRadioButton = new System.Windows.Forms.RadioButton();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.selectionAreaTextBox = new System.Windows.Forms.TextBox();
            this.workspacePanel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.leftPanel.SuspendLayout();
            this.toolGroupBox.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.imageToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1022, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(158, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resizeToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(78, 29);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // resizeToolStripMenuItem
            // 
            this.resizeToolStripMenuItem.Name = "resizeToolStripMenuItem";
            this.resizeToolStripMenuItem.Size = new System.Drawing.Size(162, 34);
            this.resizeToolStripMenuItem.Text = "Resize";
            this.resizeToolStripMenuItem.Click += new System.EventHandler(this.resizeToolStripMenuItem_Click);
            // 
            // pickColorBtn
            // 
            this.pickColorBtn.Location = new System.Drawing.Point(3, 3);
            this.pickColorBtn.Name = "pickColorBtn";
            this.pickColorBtn.Size = new System.Drawing.Size(93, 49);
            this.pickColorBtn.TabIndex = 2;
            this.pickColorBtn.TabStop = false;
            this.pickColorBtn.Text = "Pick color";
            this.pickColorBtn.UseVisualStyleBackColor = true;
            this.pickColorBtn.Click += new System.EventHandler(this.pickColorBtn_Click);
            // 
            // colorDisplayTextBox
            // 
            this.colorDisplayTextBox.BackColor = System.Drawing.Color.Black;
            this.colorDisplayTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorDisplayTextBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.colorDisplayTextBox.Enabled = false;
            this.colorDisplayTextBox.ForeColor = System.Drawing.SystemColors.Window;
            this.colorDisplayTextBox.Location = new System.Drawing.Point(102, 15);
            this.colorDisplayTextBox.Name = "colorDisplayTextBox";
            this.colorDisplayTextBox.Size = new System.Drawing.Size(26, 26);
            this.colorDisplayTextBox.TabIndex = 3;
            this.colorDisplayTextBox.TabStop = false;
            // 
            // leftPanel
            // 
            this.leftPanel.AutoSize = true;
            this.leftPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.leftPanel.BackColor = System.Drawing.SystemColors.Control;
            this.leftPanel.Controls.Add(this.toolGroupBox);
            this.leftPanel.Controls.Add(this.pickColorBtn);
            this.leftPanel.Controls.Add(this.colorDisplayTextBox);
            this.leftPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.leftPanel.Location = new System.Drawing.Point(0, 33);
            this.leftPanel.MinimumSize = new System.Drawing.Size(138, 0);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(138, 497);
            this.leftPanel.TabIndex = 5;
            // 
            // toolGroupBox
            // 
            this.toolGroupBox.Controls.Add(this.bucketRadioButton);
            this.toolGroupBox.Controls.Add(this.boxRadioButton);
            this.toolGroupBox.Controls.Add(this.drawRadioButton);
            this.toolGroupBox.Controls.Add(this.eraseRadioButton);
            this.toolGroupBox.Location = new System.Drawing.Point(12, 58);
            this.toolGroupBox.Name = "toolGroupBox";
            this.toolGroupBox.Size = new System.Drawing.Size(116, 160);
            this.toolGroupBox.TabIndex = 7;
            this.toolGroupBox.TabStop = false;
            this.toolGroupBox.Text = "Tools";
            // 
            // bucketRadioButton
            // 
            this.bucketRadioButton.AutoSize = true;
            this.bucketRadioButton.Location = new System.Drawing.Point(12, 116);
            this.bucketRadioButton.Name = "bucketRadioButton";
            this.bucketRadioButton.Size = new System.Drawing.Size(84, 24);
            this.bucketRadioButton.TabIndex = 8;
            this.bucketRadioButton.Text = "Bucket";
            this.bucketRadioButton.UseVisualStyleBackColor = true;
            this.bucketRadioButton.CheckedChanged += new System.EventHandler(this.toolSelectionRadioButton_CheckedChanged);
            // 
            // boxRadioButton
            // 
            this.boxRadioButton.AutoSize = true;
            this.boxRadioButton.Location = new System.Drawing.Point(12, 85);
            this.boxRadioButton.Name = "boxRadioButton";
            this.boxRadioButton.Size = new System.Drawing.Size(61, 24);
            this.boxRadioButton.TabIndex = 7;
            this.boxRadioButton.TabStop = true;
            this.boxRadioButton.Text = "Box";
            this.boxRadioButton.UseVisualStyleBackColor = true;
            // 
            // drawRadioButton
            // 
            this.drawRadioButton.AutoSize = true;
            this.drawRadioButton.Checked = true;
            this.drawRadioButton.Location = new System.Drawing.Point(12, 25);
            this.drawRadioButton.Name = "drawRadioButton";
            this.drawRadioButton.Size = new System.Drawing.Size(71, 24);
            this.drawRadioButton.TabIndex = 5;
            this.drawRadioButton.TabStop = true;
            this.drawRadioButton.Text = "Draw";
            this.drawRadioButton.UseVisualStyleBackColor = true;
            this.drawRadioButton.CheckedChanged += new System.EventHandler(this.toolSelectionRadioButton_CheckedChanged);
            // 
            // eraseRadioButton
            // 
            this.eraseRadioButton.AutoSize = true;
            this.eraseRadioButton.Location = new System.Drawing.Point(12, 55);
            this.eraseRadioButton.Name = "eraseRadioButton";
            this.eraseRadioButton.Size = new System.Drawing.Size(76, 24);
            this.eraseRadioButton.TabIndex = 6;
            this.eraseRadioButton.Text = "Erase";
            this.eraseRadioButton.UseVisualStyleBackColor = true;
            this.eraseRadioButton.CheckedChanged += new System.EventHandler(this.toolSelectionRadioButton_CheckedChanged);
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.White;
            this.bottomPanel.Controls.Add(this.selectionAreaTextBox);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Location = new System.Drawing.Point(0, 530);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(1022, 30);
            this.bottomPanel.TabIndex = 3;
            // 
            // selectionAreaTextBox
            // 
            this.selectionAreaTextBox.BackColor = System.Drawing.Color.White;
            this.selectionAreaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.selectionAreaTextBox.Location = new System.Drawing.Point(266, 3);
            this.selectionAreaTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.selectionAreaTextBox.Name = "selectionAreaTextBox";
            this.selectionAreaTextBox.ReadOnly = true;
            this.selectionAreaTextBox.Size = new System.Drawing.Size(462, 19);
            this.selectionAreaTextBox.TabIndex = 0;
            this.selectionAreaTextBox.TabStop = false;
            // 
            // workspacePanel
            // 
            this.workspacePanel.AutoScroll = true;
            this.workspacePanel.AutoSize = true;
            this.workspacePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.workspacePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workspacePanel.Location = new System.Drawing.Point(138, 33);
            this.workspacePanel.Name = "workspacePanel";
            this.workspacePanel.Size = new System.Drawing.Size(884, 497);
            this.workspacePanel.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 560);
            this.Controls.Add(this.workspacePanel);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Image Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            this.leftPanel.PerformLayout();
            this.toolGroupBox.ResumeLayout(false);
            this.toolGroupBox.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.Button pickColorBtn;
        private System.Windows.Forms.TextBox colorDisplayTextBox;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resizeToolStripMenuItem;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.RadioButton drawRadioButton;
        private System.Windows.Forms.RadioButton eraseRadioButton;
        private System.Windows.Forms.GroupBox toolGroupBox;
        private System.Windows.Forms.RadioButton boxRadioButton;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.RadioButton bucketRadioButton;
        public System.Windows.Forms.TextBox selectionAreaTextBox;
        private System.Windows.Forms.Panel workspacePanel;
    }
}

