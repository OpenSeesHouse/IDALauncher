using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace IDAClient
{
    public partial class ClientForm : Form
    {
        private TextBox _textBox1;
        private DataGridView _jobsGridView;
        private Button _hideBtn;

        public static string Folder { set; get; }
        public static string ServerIp { set; get; }

        public int MaxCpuUsage { get; set; }

        delegate void ChngCnctStatDelegate(bool cnctd, int ncore);
        delegate void AddMsgDelegate(string msg);
        delegate void EditJobTableDelegate(string model, string rec, string im, string status, string core, string start, string end, string t);
        delegate void ClientStartDelegate(ClientManager mngr);

        public void ChangeFormTitle(bool cnctd, int nCores)
        {
            var title = "IDA Client " + (cnctd ? "(Connected)" : "(Disconnect.)") + $" Running with {nCores} cores";
            if (!InvokeRequired)
                Text = title;
            else
            {
                var dlgt = new ChngCnctStatDelegate(ChangeFormTitle);
                Invoke(dlgt, cnctd, nCores);
            }
        }
        public void AddMsgToBox(string msg)
        {
            if (_textBox1.InvokeRequired == false)
            {
                _textBox1.AppendText(msg);
            }
            else
            {
                // Show progress asynchronously
                var addMsg = new AddMsgDelegate(AddMsgToBox);
                Invoke(addMsg, msg);
            }
        }


        public void UpdateJobInTable(string model, string rec, string im, string state, string core, string start, string end, string t)
        {
            
            if (_jobsGridView.InvokeRequired == false)
            {
                var ind = -1;
                foreach (DataGridViewRow row in _jobsGridView.Rows)
                {
                    if (String.CompareOrdinal(row.Cells["Model"].Value.ToString() , model) != 0)
                        continue;
                    if (String.CompareOrdinal(row.Cells["Record"].Value.ToString(), rec) != 0)
                        continue;
                    if (String.CompareOrdinal(row.Cells["IM"].Value.ToString(), im) != 0)
                        continue;
                    ind = row.Index;
                    break;
                }
                if (ind == -1)
                {
                    _jobsGridView.Rows.Insert(0, core, state, model, rec, im, start, end, t);
                    _jobsGridView.Rows[0].Height = 20;
                    _jobsGridView.Rows[0].HeaderCell.Value = _jobsGridView.RowCount;
                }
                else
                {
                    _jobsGridView.Rows[ind].Cells["Core Id."].Value = core;
                    _jobsGridView.Rows[ind].Cells["Status"].Value = state;
                    _jobsGridView.Rows[ind].Cells["Model"].Value = model;
                    _jobsGridView.Rows[ind].Cells["IM"].Value = im;
                    _jobsGridView.Rows[ind].Cells["Start"].Value = start;
                    _jobsGridView.Rows[ind].Cells["End"].Value = end;
                    _jobsGridView.Rows[ind].Cells["Duration"].Value = t;
                }
            }
            else
            {
                // Show progress asynchronously
                var addMsg = new EditJobTableDelegate(UpdateJobInTable);
                Invoke(addMsg, model, rec, im, state, core, start, end, t);
            }
        }

        private static void Start(ClientManager mngr)
        {
            mngr.Manage();
        }
        public ClientForm()
        {
            InitializeComponent();
            StartBtn.Enabled = false;
            Logger.MyForm = this;
            ServerIp = "127.0.0.1";
        }


        private void StartBtn_Click(object sender, EventArgs e)
        {
            ServerIp = IPTextBox.Text;
            IPTextBox.Enabled = false;
            BrwsWDirBtn.Enabled = false;
            WDirText.Enabled = false;
            ClientSize = new Size(529, 550);
            // 
            // textBox1
            // 
            _textBox1 = new TextBox
            {
                Location = new Point(14, 440),
                Multiline = true,
                Name = "textBox1",
                Size = new Size(510, 100),
                TabIndex = 15
            };
            Controls.Add(_textBox1);
            // 
            // JobsGridView
            // 
            _jobsGridView = new DataGridView
            {
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize,
                Location = new Point(14, 133),
                Name = "JobsGridView",
                Size = new Size(510, 301),
                TabIndex = 16,
                ColumnCount = 8,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersDefaultCellStyle = { Alignment = DataGridViewContentAlignment.TopLeft },
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.TopLeft }
            };
            Controls.Add(_jobsGridView);
            _jobsGridView.Columns[0].Frozen = true;
            _jobsGridView.Columns[1].Frozen = true;
            _jobsGridView.Columns[2].Frozen = true;
            _jobsGridView.Columns[3].Frozen = true;
            _jobsGridView.Columns[4].Frozen = true;
            foreach (DataGridViewColumn col in _jobsGridView.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.ReadOnly = true;
            }
            _jobsGridView.Columns[0].HeaderCell.Value = "Core Id.";
            _jobsGridView.Columns[1].HeaderCell.Value = "Status";
            _jobsGridView.Columns[2].HeaderCell.Value = "Model";
            _jobsGridView.Columns[3].HeaderCell.Value = "Record";
            _jobsGridView.Columns[4].HeaderCell.Value = "IM";
            _jobsGridView.Columns[5].HeaderCell.Value = "Start";
            _jobsGridView.Columns[6].HeaderCell.Value = "End";
            _jobsGridView.Columns[7].HeaderCell.Value = "Duration";

            _jobsGridView.Columns[0].Name = "Core Id.";
            _jobsGridView.Columns[1].Name = "Status";
            _jobsGridView.Columns[2].Name = "Model";
            _jobsGridView.Columns[3].Name = "Record";
            _jobsGridView.Columns[4].Name = "IM";
            _jobsGridView.Columns[5].Name = "Start";
            _jobsGridView.Columns[6].Name = "End";
            _jobsGridView.Columns[7].Name = "Duration";

            // 
            // HideBtn
            // 
            _hideBtn = new Button
            {
                Location = new Point(15, 100),
                Name = "HideBtn",
                Size = new Size(60, 23),
                TabIndex = 15,
                Text = "Hide",
                UseVisualStyleBackColor = true
            };
            Controls.Add(_hideBtn);
            _hideBtn.Click += HideBtnOnClick;

            try
            {
                MaxCpuUsage = Convert.ToInt16(MaxCpuPercentNumeric.Value);
            }
            catch 
            {
                MaxCpuUsage = 100;
            }
            var theClientMngr = new ClientManager(Folder, ServerIp, MaxCpuUsage);
            var start = new ClientStartDelegate(Start);
            start.BeginInvoke(theClientMngr, null, null);
        }

        private void HideBtnOnClick(object sender, EventArgs eventArgs)
        {
            if (_hideBtn.Text == "Hide")
            {
                _hideBtn.Text = "UnHide";
                ClientSize = new Size(529, 130);
            }
            else
            {
                _hideBtn.Text = "Hide";
                ClientSize = new Size(529, 550);

            }
        }

        private void BrowseWDir_Click(object sender, EventArgs e)
        {
            var folderBrowser = new OpenFileDialog
            {
                InitialDirectory = Folder,
                Filter = "all|*.*",
                FilterIndex = 0,
                RestoreDirectory = true,
                Title = "Select the folder where your Model Directory is placed in",
                DefaultExt = "",
                CheckFileExists = false,
                FileName = "select current folder"
            };
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                Folder = folderBrowser.FileName;
                
                var ind = Folder.LastIndexOfAny(new[] { '/', '\\' });
                Folder = Folder.Substring(0, ind);
                WDirText.Text = Folder;
                StartBtn.Enabled = true;
                OpenWdirBtn.Enabled = true;
            }
        }

        private void MaxCpuPercentNumeric_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                MaxCpuUsage = Convert.ToInt16(MaxCpuPercentNumeric.Text);
            }
            catch
            {
                MaxCpuUsage = 100;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void OpenWdirBtn_Click(object sender, EventArgs e)
        {
            Process.Start(Folder);
        }
    }
}
