using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities;
namespace IDALauncher
{
    public partial class TruncMTForm : Form
    {
        private delegate void showResultsDelegate(int iRec, int nPnt, double sa, double disp, double slope,
            double slopeRat, int clpsFlag, int failureFlag, double endTime, string stage, string thrd);

        private delegate void addRecordTabDelegate(int rec);
        TruncHuntFillTracor TheTracor;
        private string TclModelPath;
        private string OpsExePath;
        int NumThreads;
        List<Thread> ThreadList;
        Dictionary<int,DataGridView> DGWs = new Dictionary<int, DataGridView>();
        public TruncMTForm(double saStart, double saStep, double saStepIncr, List<int> recList, int numThreads, string tclPath, string opsPath)
        {

            InitializeComponent();
            TheTracor = new TruncHuntFillTracor(saStart, saStep, saStepIncr, recList);
            TclModelPath = tclPath;
            OpsExePath = opsPath;
            NumThreads = numThreads;
            ThreadList = new List<Thread>(NumThreads);
            foreach (var rec in recList)
                addRecordTab(rec);
        }

        private void addRecordTab(int rec)
        {
            if (InvokeRequired)
            {
                var d = new addRecordTabDelegate(addRecordTab);
                Invoke(d, new object[] { rec });
            }
            else
            {
                tabControl1.SuspendLayout();
                this.SuspendLayout();

                var tp = new TabPage();
                tp.SuspendLayout();
                tp.Location = new Point(4, 22);
                tp.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                tp.Name = $"tp{rec}";
                tp.Padding = new Padding(3);
                tp.MaximumSize = new Size(4550, 2040);
                tp.Size = new Size(455, 204);
                tp.TabIndex = 0;
                tp.Text = $"Rec-{rec}";
                tp.UseVisualStyleBackColor = true;
                tabControl1.Controls.Add(tp);

                var dgw = new DataGridView();
                ((ISupportInitialize)(dgw)).BeginInit();
                tp.Controls.Add(dgw);
                dgw.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                dgw.MaximumSize = new Size(4490, 1980);
                dgw.AllowUserToAddRows = false;
                dgw.AllowUserToDeleteRows = false;
                dgw.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgw.Dock = DockStyle.Fill;
                dgw.Location = new Point(3, 3);
                dgw.Name = $"DGW{rec}";
                dgw.ReadOnly = true;
                dgw.Size = new Size(449, 198);
                dgw.TabIndex = 0;

                var clmn1 = new DataGridViewTextBoxColumn();
                clmn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn1.Frozen = true;
                clmn1.HeaderText = "Num";
                clmn1.Name = "Num";
                clmn1.ReadOnly = true;
                clmn1.Width = 25;

                var clmn2 = new DataGridViewTextBoxColumn();
                clmn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn2.Frozen = false;
                clmn2.HeaderText = "IM";
                clmn2.Name = "IM";
                clmn2.ReadOnly = true;
                clmn2.Width = 75;

                var clmn3 = new DataGridViewTextBoxColumn();
                clmn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn3.Frozen = false;
                clmn3.HeaderText = "SlopeRat";
                clmn3.Name = "SR";
                clmn3.ReadOnly = true;
                clmn3.Width = 75;

                var clmn4 = new DataGridViewTextBoxColumn();
                clmn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn4.Frozen = false;
                clmn4.HeaderText = "Collapsed";
                clmn4.Name = "Clps";
                clmn4.ReadOnly = true;
                clmn4.Width = 40;

                var clmn5 = new DataGridViewTextBoxColumn();
                clmn5.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn5.Frozen = false;
                clmn5.HeaderText = "Diverged";
                clmn5.Name = "Divrgd";
                clmn5.ReadOnly = true;
                clmn5.Width = 40;

                var clmn6 = new DataGridViewTextBoxColumn();
                clmn6.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn6.Frozen = false;
                clmn6.HeaderText = "EndTime";
                clmn6.Name = "EndT";
                clmn6.ReadOnly = true;
                clmn6.Width = 40;

                var clmn7 = new DataGridViewTextBoxColumn();
                clmn7.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn7.Frozen = false;
                clmn7.HeaderText = "Stage";
                clmn7.Name = "Stage";
                clmn7.ReadOnly = true;
                clmn7.Width = 40;

                var clmn8 = new DataGridViewTextBoxColumn();
                clmn8.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn8.Frozen = false;
                clmn8.HeaderText = "EDP";
                clmn8.Name = "EDP";
                clmn8.ReadOnly = true;
                clmn8.Width = 75;

                var clmn9 = new DataGridViewTextBoxColumn();
                clmn9.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                clmn9.Frozen = false;
                clmn9.HeaderText = "Thrd";
                clmn9.Name = "Thrd";
                clmn9.ReadOnly = true;
                clmn9.Width = 40;

                //num, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage
                dgw.Columns.AddRange(new DataGridViewColumn[] { clmn1, clmn2, clmn8, clmn3, clmn4, clmn5, clmn6, clmn7, clmn9 });
                ((ISupportInitialize)(dgw)).EndInit();

                tp.ResumeLayout(false);
                DGWs[rec] = dgw;
                ResumeLayout(false);
            }
        }
        internal void startTrace()
        {
            var watch = Stopwatch.StartNew();
            var strt = DateTime.Now;
            Utils.initiateConsole();
            for (var i = 0; i < NumThreads; i++)
            {
                var th = new Thread(new ThreadStart(newIdaTask));
                th.Name = $"{i + 1}";
                ThreadList.Add(th);
                th.Start();
            }
            foreach (var th in ThreadList)
                th.Join();
            var idaOut = File.CreateText("truncIDA/overall.txt");
            foreach (var line in TheTracor.OveralOutput)
                idaOut.WriteLine(line);
            idaOut.Close();
            watch.Stop();
            var processTime = watch.Elapsed;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\nIDA started at: {0}", strt);
            Console.WriteLine("IDA ended at: {0}", DateTime.Now);
            Console.WriteLine("Total IDA time = {0}", watch.Elapsed);

        }

        private void newIdaTask()
        {
            var thName = Thread.CurrentThread.Name;
            var wdir = Directory.GetParent(TclModelPath).FullName;
            var tclModelName = Path.GetFileName(TclModelPath);
            IDATaskData task = TheTracor.GetNewTask();
            if (task == null)
                if (TheTracor.TraceFinished)
                {
                    return;
                }
                else
                {
                    Thread.Sleep(60000);
                    newIdaTask();
                }
            IDATaskResult result = new IDATaskResult();
            var res = Utils.RunOps(task.Record, task.IM, wdir, OpsExePath, tclModelName, ref result.FailFlag,
                ref result.Disp, ref result.EndTime);
            if (!res)
            {
                Console.WriteLine($"ERROR Running OpenSees at Sa= {task.IM} for record= {task.Record}");
                return;
            }
            var data = (RecordTraceData) TheTracor.SetNewResult(task, result);
            showResults(task.Record, data.n, task.IM, result.Disp, data.slope, data.slopeRat, data.clpsFlag,
                data.failureFlag, data.endTime, data.stage, thName);
            newIdaTask();
        }
        private void showResults(int iRec, int nPnt, double sa, double disp, double slope, double slopeRat, int clpsFlag, int failureFlag, double endTime, string stage, string thrd)
        {
            //num, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage
            var dgw = DGWs[iRec];
            if (dgw.InvokeRequired)
            {
                var d = new showResultsDelegate(showResults);
                dgw.Invoke(d, new object[] {iRec, nPnt, sa, disp, slope, slopeRat, clpsFlag, failureFlag, endTime, stage, thrd });
            }
            else
            {
                dgw.Rows.Add(nPnt, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage, thrd);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            Thread th = new Thread(new ThreadStart(startTrace));
            th.Start();
            Thread.Sleep(1000);
        }

    }
}
