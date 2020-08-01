namespace IDALauncher
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
            this.recRangeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.LaunchBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.HuntFillRadio = new System.Windows.Forms.RadioButton();
            this.label14 = new System.Windows.Forms.Label();
            this.HUntFillGB = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.NumThreadsTB = new System.Windows.Forms.TextBox();
            this.FullTRuncGB = new System.Windows.Forms.GroupBox();
            this.TruncRB = new System.Windows.Forms.RadioButton();
            this.FullRB = new System.Windows.Forms.RadioButton();
            this.GoFillCB = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.OpsPathTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.BrowsExeBtn = new System.Windows.Forms.Button();
            this.BrowseTCLBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.TCLPathTB = new System.Windows.Forms.TextBox();
            this.HUntFillGB.SuspendLayout();
            this.FullTRuncGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // recRangeBox
            // 
            this.recRangeBox.Location = new System.Drawing.Point(17, 211);
            this.recRangeBox.Margin = new System.Windows.Forms.Padding(4);
            this.recRangeBox.Name = "recRangeBox";
            this.recRangeBox.Size = new System.Drawing.Size(455, 22);
            this.recRangeBox.TabIndex = 0;
            this.recRangeBox.Text = "1, 2, 3";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 190);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Record Range: e.g: 1,3-5,7";
            // 
            // LaunchBtn
            // 
            this.LaunchBtn.Location = new System.Drawing.Point(17, 616);
            this.LaunchBtn.Margin = new System.Windows.Forms.Padding(4);
            this.LaunchBtn.Name = "LaunchBtn";
            this.LaunchBtn.Size = new System.Drawing.Size(455, 42);
            this.LaunchBtn.TabIndex = 2;
            this.LaunchBtn.Text = "Launch";
            this.LaunchBtn.UseVisualStyleBackColor = true;
            this.LaunchBtn.Click += new System.EventHandler(this.LaunchBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 27);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Max. Number of Points";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 251);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Start IM";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 283);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "IM step";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(380, 27);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(49, 22);
            this.textBox2.TabIndex = 6;
            this.textBox2.Text = "20";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(401, 251);
            this.textBox3.Margin = new System.Windows.Forms.Padding(2);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(47, 22);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "0.05";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(401, 283);
            this.textBox4.Margin = new System.Windows.Forms.Padding(2);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(47, 22);
            this.textBox4.TabIndex = 8;
            this.textBox4.Text = "0.05";
            // 
            // HuntFillRadio
            // 
            this.HuntFillRadio.AutoSize = true;
            this.HuntFillRadio.Checked = true;
            this.HuntFillRadio.Location = new System.Drawing.Point(6, 0);
            this.HuntFillRadio.Name = "HuntFillRadio";
            this.HuntFillRadio.Size = new System.Drawing.Size(17, 16);
            this.HuntFillRadio.TabIndex = 18;
            this.HuntFillRadio.TabStop = true;
            this.HuntFillRadio.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 312);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(90, 17);
            this.label14.TabIndex = 19;
            this.label14.Text = "Tracor Type:";
            // 
            // HUntFillGB
            // 
            this.HUntFillGB.Controls.Add(this.label17);
            this.HUntFillGB.Controls.Add(this.label19);
            this.HUntFillGB.Controls.Add(this.NumThreadsTB);
            this.HUntFillGB.Controls.Add(this.FullTRuncGB);
            this.HUntFillGB.Controls.Add(this.GoFillCB);
            this.HUntFillGB.Controls.Add(this.HuntFillRadio);
            this.HUntFillGB.Controls.Add(this.textBox1);
            this.HUntFillGB.Controls.Add(this.label2);
            this.HUntFillGB.Controls.Add(this.label18);
            this.HUntFillGB.Controls.Add(this.textBox2);
            this.HUntFillGB.Location = new System.Drawing.Point(20, 344);
            this.HUntFillGB.Name = "HUntFillGB";
            this.HUntFillGB.Size = new System.Drawing.Size(452, 265);
            this.HUntFillGB.TabIndex = 20;
            this.HUntFillGB.TabStop = false;
            this.HUntFillGB.Text = "     Hunt-Fill";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(431, 62);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(16, 17);
            this.label17.TabIndex = 25;
            this.label17.Text = "g";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(23, 218);
            this.label19.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(114, 17);
            this.label19.TabIndex = 27;
            this.label19.Text = "Num. of Threads";
            // 
            // NumThreadsTB
            // 
            this.NumThreadsTB.Location = new System.Drawing.Point(380, 218);
            this.NumThreadsTB.Margin = new System.Windows.Forms.Padding(2);
            this.NumThreadsTB.Name = "NumThreadsTB";
            this.NumThreadsTB.Size = new System.Drawing.Size(49, 22);
            this.NumThreadsTB.TabIndex = 28;
            this.NumThreadsTB.Text = "1";
            // 
            // FullTRuncGB
            // 
            this.FullTRuncGB.Controls.Add(this.TruncRB);
            this.FullTRuncGB.Controls.Add(this.FullRB);
            this.FullTRuncGB.Location = new System.Drawing.Point(26, 122);
            this.FullTRuncGB.Name = "FullTRuncGB";
            this.FullTRuncGB.Size = new System.Drawing.Size(200, 72);
            this.FullTRuncGB.TabIndex = 26;
            this.FullTRuncGB.TabStop = false;
            this.FullTRuncGB.Text = "Full/Trunctaed";
            // 
            // TruncRB
            // 
            this.TruncRB.AutoSize = true;
            this.TruncRB.Location = new System.Drawing.Point(91, 33);
            this.TruncRB.Name = "TruncRB";
            this.TruncRB.Size = new System.Drawing.Size(94, 21);
            this.TruncRB.TabIndex = 20;
            this.TruncRB.Text = "Truncated";
            this.TruncRB.UseVisualStyleBackColor = true;
            // 
            // FullRB
            // 
            this.FullRB.AutoSize = true;
            this.FullRB.Checked = true;
            this.FullRB.Location = new System.Drawing.Point(14, 33);
            this.FullRB.Name = "FullRB";
            this.FullRB.Size = new System.Drawing.Size(51, 21);
            this.FullRB.TabIndex = 19;
            this.FullRB.TabStop = true;
            this.FullRB.Text = "Full";
            this.FullRB.UseVisualStyleBackColor = true;
            // 
            // GoFillCB
            // 
            this.GoFillCB.AutoSize = true;
            this.GoFillCB.Checked = true;
            this.GoFillCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GoFillCB.Location = new System.Drawing.Point(25, 95);
            this.GoFillCB.Name = "GoFillCB";
            this.GoFillCB.Size = new System.Drawing.Size(70, 21);
            this.GoFillCB.TabIndex = 25;
            this.GoFillCB.Text = "Go Fill";
            this.GoFillCB.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(380, 61);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(49, 22);
            this.textBox1.TabIndex = 24;
            this.textBox1.Text = "0.05";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(22, 61);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(121, 17);
            this.label18.TabIndex = 23;
            this.label18.Text = "IM Step Increment";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(450, 252);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(16, 17);
            this.label15.TabIndex = 21;
            this.label15.Text = "g";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(450, 284);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(16, 17);
            this.label16.TabIndex = 22;
            this.label16.Text = "g";
            // 
            // OpsPathTB
            // 
            this.OpsPathTB.Location = new System.Drawing.Point(13, 26);
            this.OpsPathTB.Margin = new System.Windows.Forms.Padding(4);
            this.OpsPathTB.Multiline = true;
            this.OpsPathTB.Name = "OpsPathTB";
            this.OpsPathTB.Size = new System.Drawing.Size(382, 52);
            this.OpsPathTB.TabIndex = 23;
            this.OpsPathTB.Text = "C:/Program Files/OpenSees-CSS/OpenSees.exe";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 5);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(138, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "OpenSees.exe Path:";
            // 
            // BrowsExeBtn
            // 
            this.BrowsExeBtn.Location = new System.Drawing.Point(406, 26);
            this.BrowsExeBtn.Name = "BrowsExeBtn";
            this.BrowsExeBtn.Size = new System.Drawing.Size(66, 52);
            this.BrowsExeBtn.TabIndex = 25;
            this.BrowsExeBtn.Text = "Browse";
            this.BrowsExeBtn.UseVisualStyleBackColor = true;
            this.BrowsExeBtn.Click += new System.EventHandler(this.BrowsExeBtn_Click);
            // 
            // BrowseTCLBtn
            // 
            this.BrowseTCLBtn.Location = new System.Drawing.Point(407, 129);
            this.BrowseTCLBtn.Name = "BrowseTCLBtn";
            this.BrowseTCLBtn.Size = new System.Drawing.Size(66, 52);
            this.BrowseTCLBtn.TabIndex = 28;
            this.BrowseTCLBtn.Text = "Browse";
            this.BrowseTCLBtn.UseVisualStyleBackColor = true;
            this.BrowseTCLBtn.Click += new System.EventHandler(this.BrowseTCLBtn_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(12, 91);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(461, 34);
            this.label6.TabIndex = 27;
            this.label6.Text = "tcl file for dynamic analysis of your model (see runNTH.tcl for descriptions):";
            // 
            // TCLPathTB
            // 
            this.TCLPathTB.Location = new System.Drawing.Point(14, 129);
            this.TCLPathTB.Margin = new System.Windows.Forms.Padding(4);
            this.TCLPathTB.Multiline = true;
            this.TCLPathTB.Name = "TCLPathTB";
            this.TCLPathTB.Size = new System.Drawing.Size(382, 52);
            this.TCLPathTB.TabIndex = 26;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(497, 670);
            this.Controls.Add(this.BrowseTCLBtn);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.TCLPathTB);
            this.Controls.Add(this.BrowsExeBtn);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.OpsPathTB);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.HUntFillGB);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LaunchBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.recRangeBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Hunt-Fill IDA";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.HUntFillGB.ResumeLayout(false);
            this.HUntFillGB.PerformLayout();
            this.FullTRuncGB.ResumeLayout(false);
            this.FullTRuncGB.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox recRangeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button LaunchBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.RadioButton HuntFillRadio;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox HUntFillGB;
        private System.Windows.Forms.GroupBox FullTRuncGB;
        private System.Windows.Forms.RadioButton TruncRB;
        private System.Windows.Forms.RadioButton FullRB;
        private System.Windows.Forms.CheckBox GoFillCB;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox NumThreadsTB;
        private System.Windows.Forms.TextBox OpsPathTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button BrowsExeBtn;
        private System.Windows.Forms.Button BrowseTCLBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TCLPathTB;
    }
}

