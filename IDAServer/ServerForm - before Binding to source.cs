using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDAServer
{
    public partial class ServerForm : Form
    {
        private Dictionary<IdaJob, List<IdaPoint>> IdaGraphDic = new Dictionary<IdaJob, List<IdaPoint>>();
        private Dictionary<IdaClient, List<SingleRunJob>> ClientJobDic = new Dictionary<IdaClient, List<SingleRunJob>>();
        private Dictionary<IdaClient, List<ClientActivityLog>> ClientActivityDic = new Dictionary<IdaClient, List<ClientActivityLog>>();
        private delegate void ServerStartDelegate();
        delegate void AddMsgDelegate(string msg);
        delegate void SetIdaTableEntryDelegate(IdaJob idajob);
        delegate void SetClientTableEntryDelegate(IdaClient clientdata);

        private static void ServerStart()
        {
            Server.NumAllRecs = 44;
            Server.WorkingDir = @"C:\Users\Alireza\Desktop\ServerSide"; // is located in bin\Debug
            Server.ProjectFile = "Project.txt"; // is located in WorkingDir
            Server.TheTracor = new HuntFillTracor
            {
                MaxClpsRsltn = 0.05,
                TetaLimit = 0.1,
                SlopeRatioLimit = 0.2,
                Im0 = 0.05,
                Step0 = 0.1,
                StepIncr = 0.05,
                MaxIdaPnts = 20
            };

            Server.Initiate();
            Server.Run();
            Logger.Close();
        }

        public void AddMsg(string msg)
        {
            if (TheTextBox.InvokeRequired == false)
            {
                TheTextBox.AppendText(msg);
            }
            else
            {
                var addMsg = new AddMsgDelegate(AddMsg);
                try
                {
                    Invoke(addMsg, msg);
                }
                catch
                {
                    Thread.Sleep(1000);
                    Invoke(addMsg, msg);
                }
            }
        }

        public void SetIdaTableEntry(IdaJob idajob)
        {
            if (IdaTable.InvokeRequired == false)
            {
                var rowList = IdaTable.Rows.Cast<DataGridViewRow>().Where(
                    ro => ((string) ro.Cells["Model"].Value == idajob.Model) &&
                          ((string) ro.Cells["Record"].Value == idajob.Record) &&
                          ((string) ro.Cells["Folder"].Value == idajob.ModelFolder)
                          ).ToList();
                DataGridViewRow row;
                if (rowList.Count > 0)
                {
                    row = rowList[0];
                }
                else
                {
                    row = new DataGridViewRow();
                    row.Cells["Model"].Value = idajob.Model;
                    row.Cells["Record"].Value = idajob.Record;
                    row.Cells["Folder"].Value = idajob.ModelFolder;
                    IdaGraphDic[idajob] = idajob.Graph;
                }
                row.Cells["Status"].Value = idajob.Status.ToString();
                row.Cells["RunningJobs"].Value = idajob.Status.NRunningJobs;
                row.Cells["CompletedJobs"].Value = idajob.Status.NCompleteJobs;
            }
            else
            {
                var dlgt = new SetIdaTableEntryDelegate(SetIdaTableEntry);
                try
                {
                    Invoke(dlgt, idajob);
                }
                catch
                {
                    Thread.Sleep(1000);
                    Invoke(dlgt, idajob);
                }
            }
        }

        public void SetClientTableEntry(IdaClient client)
        {
            if (ClientsTable.InvokeRequired == false)
            {
                var rowList = ClientsTable.Rows.Cast<DataGridViewRow>().Where(
                    ro => (string) ro.Cells["IP"].Value == client.Ip.ToString()).ToList();
                DataGridViewRow row;
                if (rowList.Count > 0)
                {
                    row = rowList[0];
                }
                else
                {
                    row = new DataGridViewRow();
                    row.Cells["Name"].Value = client.Name;
                    row.Cells["IP"].Value = client.Ip.ToString();
                    row.Cells["NumCores"].Value = client.NCores;
                    ClientJobDic[client] = client.JobList;
                    ClientActivityDic[client] = client.ActivityLogList;
                }
                row.Cells["Connection"].Value = client.Connected? "Online" : "Offline";
                row.Cells["WorkingCores"].Value = client.NWorkingCores;
            }
            else
            {
                var dlgt = new SetIdaTableEntryDelegate(SetIdaTableEntry);
                try
                {
                    Invoke(dlgt, client);
                }
                catch
                {
                    Thread.Sleep(1000);
                    Invoke(dlgt, client);
                }
            }
        }
        
        public ServerForm()
        {
            InitializeComponent();
            InitializeSelef();
            //Logger.LogFile = @"D:\IDAServer\Log.txt";
            Logger.LogFile = "Log.txt";
            Logger.MyForm = this;
            Logger.Start();
            var start = new ServerStartDelegate(ServerStart);
            start.BeginInvoke( null, null);
            
        }

        private void InitializeSelef()
        {
            foreach (var name in new[] { "Model", "Folder", "Record", "Status", "RunningJobs", "CompleteJobs" })
            {
                IdaTable.Columns.Add(new DataGridViewColumn
                {
                    Name = name,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    Frozen = false,
                    HeaderText = name,
                    ReadOnly = true,
                });
            }
            IdaTable.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "ViewGraph",
                UseColumnTextForButtonValue = true,
                ReadOnly = true,
                HeaderText = "Graphs",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            foreach (var name in new[] { "Name", "IP", "NumCores", "Connection", "WorkingCores"})
            {
                ClientsTable.Columns.Add(new DataGridViewColumn
                {
                    Name = name,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
                    Frozen = false,
                    HeaderText = name,
                    ReadOnly = true,
                });
            }
            ClientsTable.Columns.Add(new DataGridViewButtonColumn
            {
                Name = "JobsTable",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells,
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

        }

        private void IdaFileLabel_DoubleClick(object sender, EventArgs e)
        {

        }

        private void LoadIdasButn_Click(object sender, EventArgs e)
        {

        }
    }
}
