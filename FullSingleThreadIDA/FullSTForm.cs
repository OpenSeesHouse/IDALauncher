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
    public partial class FullSTForm : Form
    {
        private delegate void showResultsDelegate(int iRec, int nPnt, double sa, double disp, double slope,
            double slopeRat, int clpsFlag, int failureFlag, double endTime, string stage);

        private delegate void addRecordTabDelegate(int rec);

        double SaStart;
        double SaStep;
        double SaStepIncr;
        int MaxNumPoints;
        List<int> RecordList;
        string TclModelPath;
        string OpsExePath;
        bool GoFill;
        Dictionary<int,DataGridView> DGWs = new Dictionary<int, DataGridView>();
        public FullSTForm(double saStart, double saStep, double saStepIncr, int maxNumPoints, bool goFill, List<int> recList, string tclPath, string opsPath)
        {

            InitializeComponent();

            SaStart = saStart;
            SaStep = saStep;
            SaStepIncr = saStepIncr;
            MaxNumPoints = maxNumPoints;
            RecordList = recList;
            TclModelPath = tclPath;
            OpsExePath = opsPath;
            GoFill = goFill;
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
            foreach (var iRec in RecordList)
            {

                var sa = SaStart;
                var clpsFlag = 0;
                var dispVec = new List<double>(MaxNumPoints + 1);
                var saVec = new List<double>(MaxNumPoints + 1);
                var lineVec = new List<string>(MaxNumPoints + 1);
                dispVec.Add(0);
                saVec.Add(0);
                lineVec.Add("0 0");
                var nPnt = 0;

                Directory.CreateDirectory(string.Format(CultureInfo.InvariantCulture, "IDAout/{0}", iRec));
                var idaOut = File.CreateText(string.Format(CultureInfo.InvariantCulture, "IDAout/{0}/IDA.txt", iRec));
                var idaOut2 = File.CreateText(string.Format(CultureInfo.InvariantCulture, "IDAout/{0}/IDAUnsorted.txt", iRec));
                var stage = Stage.Hunt;
                var saC = 0.0;
                var saNC = SaStart;
                var nFill = 1;
                var n1 = 0;
                var failureFlag = 0;
                var initSlope = 0.0;

                addRecordTab(iRec);
                while (true)
                {
                    var endTime = 0.0;
                    var disp = 0.0;
                    var res = Utils.RunOps(iRec, sa, wdir, OpsExePath, tclModelName, ref failureFlag, ref disp, ref endTime);
                    if (!res)
                    {
                        Console.WriteLine($"ERROR Running OpenSees at Sa= {sa} for record={iRec}");
                        return;
                    }
                    var folder = string.Format(CultureInfo.InvariantCulture, "IDAout/{0}/{1}", iRec, sa);

                    nPnt++;
                    saVec.Add(sa);
                    dispVec.Add(disp);
                    var slope = Math.Abs((saVec[nPnt] - saVec[n1]) / (dispVec[nPnt] - dispVec[n1]));
                    if (nPnt == 1)
                    {
                        initSlope = slope;
                    }
                    var slopeRat = slope / initSlope;
                    clpsFlag = 0;
                    if (slopeRat < 0.2 || disp > 0.1 || failureFlag == 1)
                    {
                        clpsFlag = 1;
                    }
                    var line = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7}",
                        sa, disp, slope, slopeRat, clpsFlag, failureFlag, endTime, stage);
                    showResults(iRec, nPnt, sa, disp, slope, slopeRat, clpsFlag, failureFlag, endTime, stage.ToString());
                    idaOut2.WriteLine(line);
                    idaOut2.Flush();
                    lineVec.Add(line);
                    if (clpsFlag == 1)
                    {
                        saC = sa;

                    } else {
                        saNC = sa;
                        n1 = nPnt;

                    }
                    if (stage == Stage.Hunt && clpsFlag == 1)
                    {
                        stage = Stage.Bracket;
                        nFill = nPnt - 1;

                    }
                    if (stage == Stage.Bracket)
                    {
                        var clpsRsltn = (saC - saNC) / saNC;
                        if (clpsRsltn < 0.05)
                        {
                            stage = Stage.Fill;
                        }
                    }
                    if (stage == Stage.Hunt)
                    {
                        sa = sa + SaStepIncr * (nPnt + 1);
                    }
                    else if (stage == Stage.Bracket)
                    {
                        sa = saNC + (saC - saNC) / 3;
                    }
                    else
                    {
                        if (nFill == 0 || !GoFill || nPnt > MaxNumPoints)
                        {
                            break;
                        }
                        sa = (saVec[nFill] + saVec[nFill - 1]) / 2;
                        nFill--;
                    }
                }
                Utils.SortArray(ref lineVec, ref saVec);

                for (var i = 1; i < lineVec.Count; i++)
                {
                    idaOut.WriteLine(lineVec[i]);
                }
                idaOut.Close();
                idaOut2.Close();
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
