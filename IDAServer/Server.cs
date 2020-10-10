using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using DevComponents.DotNetBar.Controls;

namespace IDAServer
{
    public partial class Server : Form
    {
        private BindingSource _idaTableSource = new BindingSource();

        private BindingSource _clientTableSource = new BindingSource();

        //private Dictionary<IdaJob, List<IdaPoint>> IdaGraphDic = new Dictionary<IdaJob, List<IdaPoint>>();
        //private Dictionary<IdaClient, List<SingleRunJob>> ClientJobDic = new Dictionary<IdaClient, List<SingleRunJob>>();
        //private Dictionary<IdaClient, List<ClientActivityLog>> ClientActivityDic = new Dictionary<IdaClient, List<ClientActivityLog>>();

        public string ProjectFile;
        public string WorkingDir;
        public ParallelTracer TheTracer;
        public List<string> AllRecordsList;
        public int NumAllRecs;

        private delegate void VoidDelegate();

        private delegate void AddMsgDelegate(string msg);

        private void Start()
        {

            if (string.IsNullOrEmpty(WorkingDir))
            {
                ShowError("Please load the IDA file");
                Thread.CurrentThread.Abort();
            }
            double maxClpsRsltn = 0;
            double tetaLimit = 0;
            double slopeRatioLimit = 0;
            double im0 = 0;
            double step0 = 0;
            double stepIncr = 0;
            int maxPnts = 0;
            try
            {
                maxClpsRsltn = Convert.ToDouble(ClpsRsltnBox.Text);
                tetaLimit = Convert.ToDouble(TetaLimitBox.Text);
                slopeRatioLimit = Convert.ToDouble(SlopeLimitBox.Text);
                im0 = Convert.ToDouble(Im0Box.Text);
                step0 = Convert.ToDouble(Step0Box.Text);
                stepIncr = Convert.ToDouble(DeltaStepBox.Text);
                maxPnts = Convert.ToInt16(NumIdaPntsBox.Text);
            }
            catch
            {
                ShowError("Error reading IDA settings. Please check input values");
                Thread.CurrentThread.Abort();
            }
            TheTracer = new HuntFillTracer
            {
                MaxClpsRsltn = maxClpsRsltn,
                TetaLimit = tetaLimit,
                SlopeRatioLimit = slopeRatioLimit,
                Im0 = im0,
                Step0 = step0,
                StepIncr = stepIncr,
                MaxIdaPnts = maxPnts
            };

            NumAllRecs = 0;
            try
            {
                NumAllRecs = Convert.ToInt16(NumRecsBox.Text);
            }
            catch
            {
                ShowError("Error reading IDA settings. Please check input values");
                Thread.CurrentThread.Abort();
            }
            AllRecordsList = new List<string>(NumAllRecs);
            for (var i = 1; i <= NumAllRecs; i++)
                AllRecordsList.Add(string.Format("{0}", i));

            JobManager.LoadProject(ProjectFile);
            JobManager.PopulateActiveIdas();
            JobManager.PopulatePool();
            ClientManager.ListenToAny();
            //ClientManager.StartHandling();
            //Logger.Close();
        }

        public void ShowError(string msg)
        {
            if (InvokeRequired == false)
            {
                MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                var dlgt = new AddMsgDelegate(ShowError);
                Invoke(dlgt, msg);
            }
        }

        public void AddStatusMsg(string msg)
        {
            if (IdaTable.InvokeRequired == false)
            {
                StatusMsg.Text = msg;
            }
            else
            {
                var dlgt = new AddMsgDelegate(AddStatusMsg);
                Invoke(dlgt);
            }
        }

        public void UpdateIdaTable()
        {
            if (IdaTable.InvokeRequired == false)
            {
                _idaTableSource.ResetBindings(false);
            }
            else
            {
                var dlgt = new VoidDelegate(UpdateIdaTable);
                Invoke(dlgt);
            }
        }

        public void UpdateClientTable()
        {
            if (ClientsTable.InvokeRequired == false)
            {
                _clientTableSource.ResetBindings(false);
            }
            else
            {
                var dlgt = new VoidDelegate(UpdateClientTable);
                Invoke(dlgt);
            }
        }

        public Server()
        {
            InitializeComponent();
            //_idaTableSource.DataSource = JobManager.IdaJobPool;
            //_clientTableSource.DataSource = ClientManager.ClientList;
            ProjectFile = "";
            WorkingDir = "";
            //Logger.LogFile = @"D:\IDAServer\Log.txt";
            Logger.LogFile = "Log.txt";
            Logger.MyForm = this;
            Logger.Start();          
        }

        private void InitializeSelef()
        {
            _idaTableSource.DataSource = JobManager.IdaJobPool;
            IdaTable.AutoGenerateColumns = false;
            IdaTable.DataSource = _idaTableSource;
            foreach (var name in new[] { "Model", "Record", "Folder", "Status", "RunningJobs", "CompleteJobs" })
            {
                IdaTable.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    DataPropertyName = name,
                    Frozen = false,
                    HeaderText = name,
                    ReadOnly = true,
                });
            }
            IdaTable.Columns["Model"].Frozen = true;
            IdaTable.Columns["Record"].Frozen = true;
            IdaTable.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ViewGraph",
                UseColumnTextForButtonValue = true,
                ReadOnly = true,
                HeaderText = "Graphs",
            });
            IdaTable.CellClick += IdaTable_CellClick;

            _clientTableSource.DataSource = ClientManager.ClientList;
            ClientsTable.AutoGenerateColumns = false;
            ClientsTable.DataSource = _clientTableSource;
            foreach (var name in new[] { "Name", "Ip", "NumCores", "Connected", "WorkingCores" })
            {
                ClientsTable.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = name,
                    DataPropertyName = name,
                    Frozen = false,
                    HeaderText = name,
                    ReadOnly = true
                });
            }
            ClientsTable.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "JobsTable",
                Frozen = false,
                HeaderText = "Jobs",
                ReadOnly = true,
            });
            ClientsTable.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ActivityHistory",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                Frozen = false,
                HeaderText = "ClientActivity",
                ReadOnly = true,
            });
            ClientsTable.Columns["Name"].Frozen = true;
            ClientsTable.CellClick += ClientsTable_CellClick;

        }

        private void ClientsTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ClientsTable.Columns["ViewGraph"] == null)
                return;
            var column = ClientsTable.Columns["JobsTable"];
            if (column != null && e.ColumnIndex == column.Index)
            {
                var ip = (string) ClientsTable.Rows[e.RowIndex].Cells["Ip"].Value;
                var client = ClientManager.GetClient(IPAddress.Parse(ip));
                ShowClientJobs(client);
                return;
            }
            column = ClientsTable.Columns["ActivityHistory"];
            if (column != null && e.ColumnIndex == column.Index)
            {
                var ip = (string)ClientsTable.Rows[e.RowIndex].Cells["Ip"].Value;
                var client = ClientManager.GetClient(IPAddress.Parse(ip));
                ShowClientActivity(client);
            }
        }

        private static void ShowClientActivity(IdaClient client)
        {
            MessageBox.Show("Client Activity Graph Will be Implemented Soon", "Sorry!", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
        }

        private static void ShowClientJobs(IdaClient client)
        {
            MessageBox.Show("Client Jobs Table Will be Implemented Soon", "Sorry!", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
        }

        private void IdaTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var column = IdaTable.Columns["ViewGraph"];
            if (column == null || e.ColumnIndex != column.Index) return;
            var model = (string)IdaTable.Rows[e.RowIndex].Cells["Model"].Value;
            var folder = (string)IdaTable.Rows[e.RowIndex].Cells["Folder"].Value;
            var rec = (string)IdaTable.Rows[e.RowIndex].Cells["Record"].Value;
            var idaJob = JobManager.GetIdaJob(model, rec, folder);
            if (idaJob == null)
                MessageBox.Show("IdaJob Not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                ShowGraph(idaJob.Graph);
            }
        }

        private static void ShowGraph(List<IdaPoint> graph)
        {
            MessageBox.Show("IDA Graph Will be Implemented Soon", "Sorry!", MessageBoxButtons.OK,
                MessageBoxIcon.Asterisk);
        }

        private void LoadIdasButn_Click(object sender, EventArgs e)
        {
            var fileBrowser = new OpenFileDialog
            {
                InitialDirectory = WorkingDir,
                Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,
                Title = "Select the IDA project file",
                DefaultExt = ".txt"
            };
            if (fileBrowser.ShowDialog() == DialogResult.OK)
            {
                ProjectFile = fileBrowser.FileName;
                IdaFileLabel.Text = ProjectFile;
                var ind = ProjectFile.LastIndexOfAny(new[] { '/', '\\' });
                WorkingDir = ProjectFile.Substring(0, ind);
                Start();
                InitializeSelef();
                UpdateIdaTable();
                UpdateClientTable();
            }

        }

        private void IdaFileLabel_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(WorkingDir);
            }
            catch
            {
                MessageBox.Show(string.Format("Invalid Address: {0}", WorkingDir), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void HuntFillRadio_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void StartTasksButn_Click(object sender, EventArgs e)
        {
            var start = new VoidDelegate(Start);
            //AsyncCallback callBack = 
            start.BeginInvoke(null, null);

        }

    }
}
