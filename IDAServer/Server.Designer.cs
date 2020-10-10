using System.Windows.Forms;

namespace IDAServer
{
    partial class Server
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.settingsPage = new System.Windows.Forms.TabPage();
            this.NumPostsBox = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NumIdaPntsBox = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.DeltaStepBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Step0Box = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Im0Box = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SlopeLimitBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.TetaLimitBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ClpsRsltnBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.HuntFillRadio = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.NumRecsBox = new System.Windows.Forms.NumericUpDown();
            this.IdaPage = new System.Windows.Forms.TabPage();
            this.IdaTable = new System.Windows.Forms.DataGridView();
            this.StartTasksButn = new System.Windows.Forms.Button();
            this.IdaFileLabel = new System.Windows.Forms.LinkLabel();
            this.LoadIdasButn = new System.Windows.Forms.Button();
            this.ClientsPage = new System.Windows.Forms.TabPage();
            this.ClientsTable = new DevComponents.DotNetBar.Controls.DataGridViewX();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusMsg = new System.Windows.Forms.ToolStripStatusLabel();
            this.label10 = new System.Windows.Forms.Label();
            this.IdaNumToVlientPercentBox = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.settingsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPostsBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumIdaPntsBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumRecsBox)).BeginInit();
            this.IdaPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IdaTable)).BeginInit();
            this.ClientsPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ClientsTable)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IdaNumToVlientPercentBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.settingsPage);
            this.tabControl1.Controls.Add(this.IdaPage);
            this.tabControl1.Controls.Add(this.ClientsPage);
            this.tabControl1.Location = new System.Drawing.Point(-2, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 1;
            this.tabControl1.Size = new System.Drawing.Size(608, 509);
            this.tabControl1.TabIndex = 1;
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.label11);
            this.settingsPage.Controls.Add(this.label10);
            this.settingsPage.Controls.Add(this.IdaNumToVlientPercentBox);
            this.settingsPage.Controls.Add(this.NumPostsBox);
            this.settingsPage.Controls.Add(this.label9);
            this.settingsPage.Controls.Add(this.groupBox1);
            this.settingsPage.Controls.Add(this.label1);
            this.settingsPage.Controls.Add(this.NumRecsBox);
            this.settingsPage.Location = new System.Drawing.Point(4, 22);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Padding = new System.Windows.Forms.Padding(3);
            this.settingsPage.Size = new System.Drawing.Size(600, 483);
            this.settingsPage.TabIndex = 2;
            this.settingsPage.Text = "Settings";
            this.settingsPage.UseVisualStyleBackColor = true;
            // 
            // NumPostsBox
            // 
            this.NumPostsBox.Location = new System.Drawing.Point(325, 26);
            this.NumPostsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumPostsBox.Name = "NumPostsBox";
            this.NumPostsBox.Size = new System.Drawing.Size(35, 20);
            this.NumPostsBox.TabIndex = 4;
            this.NumPostsBox.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(205, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Num Pre-Posted IMs";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.NumIdaPntsBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DeltaStepBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Step0Box);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Im0Box);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.SlopeLimitBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.TetaLimitBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.ClpsRsltnBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.HuntFillRadio);
            this.groupBox1.Location = new System.Drawing.Point(13, 97);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 159);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Tracer Settings";
            // 
            // NumIdaPntsBox
            // 
            this.NumIdaPntsBox.Location = new System.Drawing.Point(148, 121);
            this.NumIdaPntsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumIdaPntsBox.Name = "NumIdaPntsBox";
            this.NumIdaPntsBox.Size = new System.Drawing.Size(35, 20);
            this.NumIdaPntsBox.TabIndex = 3;
            this.NumIdaPntsBox.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 130);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Max. IDA Points";
            // 
            // DeltaStepBox
            // 
            this.DeltaStepBox.Location = new System.Drawing.Point(309, 49);
            this.DeltaStepBox.Name = "DeltaStepBox";
            this.DeltaStepBox.Size = new System.Drawing.Size(27, 20);
            this.DeltaStepBox.TabIndex = 12;
            this.DeltaStepBox.Text = "0.05";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(200, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Step Increment";
            // 
            // Step0Box
            // 
            this.Step0Box.Location = new System.Drawing.Point(156, 97);
            this.Step0Box.Name = "Step0Box";
            this.Step0Box.Size = new System.Drawing.Size(27, 20);
            this.Step0Box.TabIndex = 10;
            this.Step0Box.Text = "0.05";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Initial Step";
            // 
            // Im0Box
            // 
            this.Im0Box.Location = new System.Drawing.Point(156, 73);
            this.Im0Box.Name = "Im0Box";
            this.Im0Box.Size = new System.Drawing.Size(27, 20);
            this.Im0Box.TabIndex = 8;
            this.Im0Box.Text = "0.05";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(20, 78);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Initial Im";
            // 
            // SlopeLimitBox
            // 
            this.SlopeLimitBox.Location = new System.Drawing.Point(309, 97);
            this.SlopeLimitBox.Name = "SlopeLimitBox";
            this.SlopeLimitBox.Size = new System.Drawing.Size(27, 20);
            this.SlopeLimitBox.TabIndex = 6;
            this.SlopeLimitBox.Text = "0.2";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(200, 104);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(86, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Slope Ratio Limit";
            // 
            // TetaLimitBox
            // 
            this.TetaLimitBox.Location = new System.Drawing.Point(309, 73);
            this.TetaLimitBox.Name = "TetaLimitBox";
            this.TetaLimitBox.Size = new System.Drawing.Size(27, 20);
            this.TetaLimitBox.TabIndex = 4;
            this.TetaLimitBox.Text = "0.1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(200, 78);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Teta Limit";
            // 
            // ClpsRsltnBox
            // 
            this.ClpsRsltnBox.Location = new System.Drawing.Point(156, 49);
            this.ClpsRsltnBox.Name = "ClpsRsltnBox";
            this.ClpsRsltnBox.Size = new System.Drawing.Size(27, 20);
            this.ClpsRsltnBox.TabIndex = 2;
            this.ClpsRsltnBox.Text = "0.05";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(126, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Max. Collapse Resolution";
            // 
            // HuntFillRadio
            // 
            this.HuntFillRadio.AutoSize = true;
            this.HuntFillRadio.Checked = true;
            this.HuntFillRadio.Location = new System.Drawing.Point(11, 19);
            this.HuntFillRadio.Name = "HuntFillRadio";
            this.HuntFillRadio.Size = new System.Drawing.Size(90, 17);
            this.HuntFillRadio.TabIndex = 0;
            this.HuntFillRadio.TabStop = true;
            this.HuntFillRadio.Text = "Hunt_and_Fill";
            this.HuntFillRadio.UseVisualStyleBackColor = true;
            this.HuntFillRadio.CheckedChanged += new System.EventHandler(this.HuntFillRadio_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of records used";
            // 
            // NumRecsBox
            // 
            this.NumRecsBox.Location = new System.Drawing.Point(144, 28);
            this.NumRecsBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NumRecsBox.Name = "NumRecsBox";
            this.NumRecsBox.Size = new System.Drawing.Size(35, 20);
            this.NumRecsBox.TabIndex = 1;
            this.NumRecsBox.Value = new decimal(new int[] {
            44,
            0,
            0,
            0});
            // 
            // IdaPage
            // 
            this.IdaPage.Controls.Add(this.IdaTable);
            this.IdaPage.Controls.Add(this.StartTasksButn);
            this.IdaPage.Controls.Add(this.IdaFileLabel);
            this.IdaPage.Controls.Add(this.LoadIdasButn);
            this.IdaPage.Location = new System.Drawing.Point(4, 22);
            this.IdaPage.Name = "IdaPage";
            this.IdaPage.Padding = new System.Windows.Forms.Padding(3);
            this.IdaPage.Size = new System.Drawing.Size(600, 483);
            this.IdaPage.TabIndex = 0;
            this.IdaPage.Text = "IDA Tasks";
            this.IdaPage.UseVisualStyleBackColor = true;
            // 
            // IdaTable
            // 
            this.IdaTable.AllowUserToAddRows = false;
            this.IdaTable.AllowUserToDeleteRows = false;
            this.IdaTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.IdaTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IdaTable.Location = new System.Drawing.Point(0, 61);
            this.IdaTable.Name = "IdaTable";
            this.IdaTable.ReadOnly = true;
            this.IdaTable.Size = new System.Drawing.Size(600, 450);
            this.IdaTable.TabIndex = 10;
            // 
            // StartTasksButn
            // 
            this.StartTasksButn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.StartTasksButn.FlatAppearance.BorderSize = 3;
            this.StartTasksButn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.StartTasksButn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.StartTasksButn.Location = new System.Drawing.Point(522, 10);
            this.StartTasksButn.Name = "StartTasksButn";
            this.StartTasksButn.Size = new System.Drawing.Size(67, 45);
            this.StartTasksButn.TabIndex = 9;
            this.StartTasksButn.Text = "Start Tasks";
            this.StartTasksButn.UseVisualStyleBackColor = true;
            this.StartTasksButn.Click += new System.EventHandler(this.StartTasksButn_Click);
            // 
            // IdaFileLabel
            // 
            this.IdaFileLabel.AutoEllipsis = true;
            this.IdaFileLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IdaFileLabel.Location = new System.Drawing.Point(83, 26);
            this.IdaFileLabel.Name = "IdaFileLabel";
            this.IdaFileLabel.Size = new System.Drawing.Size(389, 24);
            this.IdaFileLabel.TabIndex = 8;
            this.IdaFileLabel.TabStop = true;
            this.IdaFileLabel.Text = "No IDA file is loaded";
            this.IdaFileLabel.Click += new System.EventHandler(this.IdaFileLabel_Click);
            // 
            // LoadIdasButn
            // 
            this.LoadIdasButn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.LoadIdasButn.FlatAppearance.BorderSize = 3;
            this.LoadIdasButn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.LoadIdasButn.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.LoadIdasButn.Location = new System.Drawing.Point(10, 10);
            this.LoadIdasButn.Name = "LoadIdasButn";
            this.LoadIdasButn.Size = new System.Drawing.Size(67, 45);
            this.LoadIdasButn.TabIndex = 7;
            this.LoadIdasButn.Text = "Load IDAs";
            this.LoadIdasButn.UseVisualStyleBackColor = true;
            this.LoadIdasButn.Click += new System.EventHandler(this.LoadIdasButn_Click);
            // 
            // ClientsPage
            // 
            //this.ClientsPage.Controls.Add(this.ClientsTable);
            this.ClientsPage.Location = new System.Drawing.Point(4, 22);
            this.ClientsPage.Name = "ClientsPage";
            this.ClientsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ClientsPage.Size = new System.Drawing.Size(600, 483);
            this.ClientsPage.TabIndex = 1;
            this.ClientsPage.Text = "Clients Control";
            this.ClientsPage.UseVisualStyleBackColor = true;
            // 
            // ClientsTable
            // 
            this.ClientsTable.AllowUserToAddRows = false;
            this.ClientsTable.AllowUserToDeleteRows = false;
            this.ClientsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ClientsTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.ClientsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ClientsTable.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(215)))), ((int)(((byte)(229)))));
            this.ClientsTable.Location = new System.Drawing.Point(3, 3);
            this.ClientsTable.Name = "ClientsTable";
            this.ClientsTable.ReadOnly = true;
            this.ClientsTable.Size = new System.Drawing.Size(594, 477);
            this.ClientsTable.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusMsg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 512);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(606, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusMsg
            // 
            this.StatusMsg.Name = "StatusMsg";
            this.StatusMsg.Size = new System.Drawing.Size(279, 17);
            this.StatusMsg.Text = "To start: Go to \"IDA Tsks\" Tab and Press \"Load IDAs\"";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(10, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(180, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Number of IDA Jobs Loaded to Pool:";
            // 
            // IdaNumToVlientPercentBox
            // 
            this.IdaNumToVlientPercentBox.Location = new System.Drawing.Point(195, 64);
            this.IdaNumToVlientPercentBox.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.IdaNumToVlientPercentBox.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.IdaNumToVlientPercentBox.Name = "IdaNumToVlientPercentBox";
            this.IdaNumToVlientPercentBox.Size = new System.Drawing.Size(41, 20);
            this.IdaNumToVlientPercentBox.TabIndex = 6;
            this.IdaNumToVlientPercentBox.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(237, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(128, 13);
            this.label11.TabIndex = 7;
            this.label11.Text = "(% of Num. Active Clients)";
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(606, 534);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Server";
            this.Text = "IDA Server";
            this.tabControl1.ResumeLayout(false);
            this.settingsPage.ResumeLayout(false);
            this.settingsPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumPostsBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumIdaPntsBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NumRecsBox)).EndInit();
            this.IdaPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.IdaTable)).EndInit();
            this.ClientsPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ClientsTable)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.IdaNumToVlientPercentBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage IdaPage;
        private System.Windows.Forms.TabPage ClientsPage;
        private DevComponents.DotNetBar.Controls.DataGridViewX ClientsTable;
        private System.Windows.Forms.LinkLabel IdaFileLabel;
        private System.Windows.Forms.Button LoadIdasButn;
        private Button StartTasksButn;
        private TabPage settingsPage;
        private GroupBox groupBox1;
        private NumericUpDown NumIdaPntsBox;
        private Label label2;
        private TextBox DeltaStepBox;
        private Label label3;
        private TextBox Step0Box;
        private Label label4;
        private TextBox Im0Box;
        private Label label5;
        private TextBox SlopeLimitBox;
        private Label label6;
        private TextBox TetaLimitBox;
        private Label label7;
        private TextBox ClpsRsltnBox;
        private Label label8;
        private RadioButton HuntFillRadio;
        private Label label1;
        private Label label9;
        public NumericUpDown NumPostsBox;
        public NumericUpDown NumRecsBox;
        private DataGridView IdaTable;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel StatusMsg;
        private Label label11;
        private Label label10;
        public NumericUpDown IdaNumToVlientPercentBox;
    }
}

