using System.Windows.Forms;

namespace IDAClient
{
    partial class ClientForm
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
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.WDirText = new System.Windows.Forms.TextBox();
            this.BrwsWDirBtn = new System.Windows.Forms.Button();
            this.OpenWdirBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.MaxCpuPercentNumeric = new System.Windows.Forms.NumericUpDown();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this._hideBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MaxCpuPercentNumeric)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(80, 28);
            this.IPTextBox.Margin = new System.Windows.Forms.Padding(1);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(106, 20);
            this.IPTextBox.TabIndex = 8;
            this.IPTextBox.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 31);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Server IP";
            // 
            // StartBtn
            // 
            this.StartBtn.BackColor = System.Drawing.Color.Gray;
            this.StartBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StartBtn.ForeColor = System.Drawing.Color.White;
            this.StartBtn.Location = new System.Drawing.Point(432, 28);
            this.StartBtn.Margin = new System.Windows.Forms.Padding(1);
            this.StartBtn.Name = "StartBtn";
            this.StartBtn.Size = new System.Drawing.Size(93, 96);
            this.StartBtn.TabIndex = 6;
            this.StartBtn.Text = "Start";
            this.StartBtn.UseVisualStyleBackColor = false;
            this.StartBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Working Dir:";
            // 
            // WDirText
            // 
            this.WDirText.Location = new System.Drawing.Point(79, 72);
            this.WDirText.Margin = new System.Windows.Forms.Padding(1);
            this.WDirText.Name = "WDirText";
            this.WDirText.Size = new System.Drawing.Size(338, 20);
            this.WDirText.TabIndex = 10;
            this.WDirText.Text = "Not set";
            // 
            // BrwsWDirBtn
            // 
            this.BrwsWDirBtn.Location = new System.Drawing.Point(284, 104);
            this.BrwsWDirBtn.Name = "BrwsWDirBtn";
            this.BrwsWDirBtn.Size = new System.Drawing.Size(60, 20);
            this.BrwsWDirBtn.TabIndex = 11;
            this.BrwsWDirBtn.Text = "Browse";
            this.BrwsWDirBtn.UseVisualStyleBackColor = true;
            this.BrwsWDirBtn.Click += new System.EventHandler(this.BrowseWDir_Click);
            // 
            // OpenWdirBtn
            // 
            this.OpenWdirBtn.Enabled = false;
            this.OpenWdirBtn.Location = new System.Drawing.Point(357, 104);
            this.OpenWdirBtn.Name = "OpenWdirBtn";
            this.OpenWdirBtn.Size = new System.Drawing.Size(60, 20);
            this.OpenWdirBtn.TabIndex = 12;
            this.OpenWdirBtn.Text = "Open";
            this.OpenWdirBtn.UseVisualStyleBackColor = true;
            this.OpenWdirBtn.Click += new System.EventHandler(this.OpenWdirBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(197, 31);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(164, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Maximum CPU usage allowed (%)";
            // 
            // MaxCpuPercentNumeric
            // 
            this.MaxCpuPercentNumeric.Location = new System.Drawing.Point(369, 29);
            this.MaxCpuPercentNumeric.Name = "MaxCpuPercentNumeric";
            this.MaxCpuPercentNumeric.Size = new System.Drawing.Size(48, 20);
            this.MaxCpuPercentNumeric.TabIndex = 14;
            this.MaxCpuPercentNumeric.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.MaxCpuPercentNumeric.ValueChanged += new System.EventHandler(this.MaxCpuPercentNumeric_ValueChanged);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(48, 48);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(2, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(529, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // _hideBtn
            // 
            this._hideBtn.Location = new System.Drawing.Point(0, 0);
            this._hideBtn.Name = "_hideBtn";
            this._hideBtn.Size = new System.Drawing.Size(75, 23);
            this._hideBtn.TabIndex = 0;
            // 
            // ClientForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(529, 136);
            this.Controls.Add(this.MaxCpuPercentNumeric);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.OpenWdirBtn);
            this.Controls.Add(this.BrwsWDirBtn);
            this.Controls.Add(this.WDirText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.StartBtn);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ClientForm";
            this.Text = "IDA Client (Disconnect.)";
            ((System.ComponentModel.ISupportInitialize)(this.MaxCpuPercentNumeric)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TextBox IPTextBox;
        private Label label1;
        private Button StartBtn;
        private Label label2;
        private TextBox WDirText;
        private Button BrwsWDirBtn;
        private Button OpenWdirBtn;
        private Label label3;
        private NumericUpDown MaxCpuPercentNumeric;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private MenuStrip menuStrip1;
    }
}

