using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Utilities;
namespace IDALauncher
{
    public partial class TruncSTForm : Form
    {
        private delegate void showResultsDelegate(int iRec, int nPnt, double sa, double disp, double slope,
            double slopeRat, int clpsFlag, int failureFlag, double endTime, string stage);

        private delegate void addRecordTabDelegate(int rec);

        double SaStart;
        double SaStep;
        double SaStepIncr;
        List<int> RecordList;
        string TclModelPath;
        string OpsExePath;
        Dictionary<int,DataGridView> DGWs = new Dictionary<int, DataGridView>();
        public TruncSTForm(double saStart, double saStep, double saStepIncr, List<int> recList, string tclPath, string opsPath)
        {

            InitializeComponent();

            SaStart = saStart;
            SaStep = saStep;
            SaStepIncr = saStepIncr;
            RecordList = recList;
            TclModelPath = tclPath;
            OpsExePath = opsPath;
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

                //num, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage
                dgw.Columns.AddRange(new DataGridViewColumn[] { clmn1, clmn2, clmn8, clmn3, clmn4, clmn5, clmn6, clmn7 });
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
            var wdir = Directory.GetParent(TclModelPath).FullName;
            var tclModelName = Path.GetFileName(TclModelPath);
            var sa = SaStart;
            var numRecs = RecordList.Count;
            var recDataDict = new Dictionary<int,RecordTraceData>(numRecs);
            foreach (var iRec in RecordList)
            {
                addRecordTab(iRec);
                recDataDict[iRec] = new RecordTraceData(iRec);
            
            }
            double sa50_ = 0;
            double sa50 = 0;
            var stage = "Hunt";
            var nPnt = 0;
            var saVec = new List<double>();
            saVec.Add(0);
            var idaOut = File.CreateText("truncIDA/overall.txt");
            while (true)
            {
                nPnt++;
                var nRec = 0;
                saVec.Add(sa);
                double clpsP = 0;
                var numClpsd = 0;
                var line = "";
                foreach (var iRec in RecordList)
                {
                    nRec++;
                    var data = recDataDict[iRec];
                    if (data.SaClps != 0 && data.SaClps < sa)
                        continue;
                    if (data.SaNoClps != 0 && data.SaNoClps > sa)
                        continue;
                    data.saVec.Add(sa);
                    data.n++;
                    var disp = 0.0;
                    var res = Utils.RunOps(iRec, sa, wdir, OpsExePath, tclModelName, ref data.failureFlag, ref disp, ref data.endTime);
                    if (!res)
                    {
                        Console.WriteLine($"ERROR Running OpenSees at Sa= {sa} for record={iRec}");
                        return;
                    }
                    data.dispVec.Add(disp);
                    var slope = Math.Abs((sa - data.saVec[data.n1]) / (disp - data.dispVec[data.n1]));
                    if (data.n == 1)
                    {
                        data.initSlope = slope;
                    }
                    data.slopeRat = slope / data.initSlope;
                    if (data.slopeRat < 0.2 || disp > 0.1 || data.failureFlag == 1)
                    {
                        data.clpsFlag = 1;
                        data.SaClps = sa;

                    }
                    else
                    {
                        data.clpsFlag = 0;
                        data.SaNoClps = sa;
                        data.n1 = data.n;

                    }
                    data.PrintSelf();
                    showResults(iRec, nPnt, sa, disp, slope, data.slopeRat, data.clpsFlag, data.failureFlag, data.endTime, stage);
                    data.Out1.WriteLine(line);
                    data.Out1.Flush();
                    data.lineVec.Add(line);
                    int numSum = 0;
                    if (stage == "Hunt")
                        numSum = numRecs;
                    else
                        numSum = nRec;
                    numClpsd = 0;
                    for (var i = 0; i < numSum; i++)
                        numClpsd += recDataDict[RecordList[i]].clpsFlag;
                    clpsP = (numClpsd + 0.0) / numRecs;

                    if (clpsP >= 0.5)
                        break;
                }

                numClpsd = 0;
                foreach (var iRec in RecordList)
                    numClpsd += recDataDict[iRec].clpsFlag;
                clpsP = (numClpsd + 0.0) / numRecs;
                line = $"{nPnt} {sa} {stage} {clpsP}";
                foreach (var iRec in RecordList)
                    line = $"{line} {recDataDict[iRec].clpsFlag}";
                idaOut.WriteLine(line);
                if (clpsP >= 0.5)
                {
                    if (stage == "Hunt")
                        stage = "Bracket";
                    sa50 = sa;

                } else
                    sa50_ = sa;
                if (stage == "Hunt")
                    sa = sa + SaStepIncr * (nPnt + 1);
                else
                {
                    var clpsRsltn = (sa50 - sa50_) / sa50_;
                    if (clpsRsltn < 0.05)
                        break;
                    sa = sa50_ + (sa50 - sa50_) / 3;
                }
            }
            idaOut.WriteLine($"MedIM= {sa50_}");
            idaOut.Close();
            foreach (var data in recDataDict.Values)
            {
                data.FinalizePrint();
            }
            watch.Stop();
            var processTime = watch.Elapsed;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\nIDA started at: {0}", strt);
            Console.WriteLine("IDA ended at: {0}", DateTime.Now);
            Console.WriteLine("Total IDA time = {0}", watch.Elapsed);

        }

        private void showResults(int iRec, int nPnt, double sa, double disp, double slope, double slopeRat, int clpsFlag, int failureFlag, double endTime, string stage)
        {
            //num, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage
            var dgw = DGWs[iRec];
            if (dgw.InvokeRequired)
            {
                var d = new showResultsDelegate(showResults);
                dgw.Invoke(d, new object[] {iRec, nPnt, sa, disp, slope, slopeRat, clpsFlag, failureFlag, endTime, stage });
            }
            else
            {
                dgw.Rows.Add(nPnt, sa, disp, slopeRat, clpsFlag, failureFlag, endTime, stage);
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
