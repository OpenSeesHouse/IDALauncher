using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDALauncher
{
    public partial class Form1 : Form
    {
        double SaStart;
        double SaStep;
        double SaStepIncr;
        int MaxNumPoints;
        List<int> RecordList;
        string TclModelPath;
        string OpsExePath;
        bool GoFill;
        public Form1(double saStart, double saStep, double saStepIncr, int maxNumPoints, bool goFill, List<int> recList, string tclPath, string opsPath)
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

        internal void startTrace()
        {
            var watch = Stopwatch.StartNew();
            var strt = DateTime.Now;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("  ******************************* OpenSeesLauncher *******************************");
            Console.WriteLine("  *                       Developed at Civil Soft Science                        *");
            Console.WriteLine("  *             www.CivilSoftScience.com,  CivilSoftScience@gmail.com            *");
            Console.WriteLine("  ********************************************************************************\n");
            foreach (var iRec in RecordList)
            {

                var wdir = Directory.GetParent(TclModelPath).FullName;
                var tclModelName = Path.GetFileName(TclModelPath);
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
                var stage = "Hunt";
                var saC = 0.0;
                var saNC = SaStart;
                var nFill = 1;
                var n1 = 0;
                var failureFlag = 0;
                var initSlope = 0.0;
                while (true)
                {
                    var endTime = 0.0;
                    label8.Text = string.Format(CultureInfo.InvariantCulture, "{0}", iRec);
                    label9.Text = string.Format(CultureInfo.InvariantCulture, "{0}", sa);
                    label11.Text = string.Format(CultureInfo.InvariantCulture, "{0}", nPnt+1);
                    label12.Text = stage;
                    this.Refresh();
                    var disp = 0.0;
                    var res = RunOps(iRec, sa, wdir, tclModelName, ref failureFlag, ref disp, ref endTime);
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
                    if (slopeRat < 0.2 || disp > 0.1 || failureFlag == 1)
                    {
                        clpsFlag = 1;
                    }
                    var line = string.Format(CultureInfo.InvariantCulture, "{0} {1} {2} {3} {4} {5} {6} {7}",
                        sa, disp, slope, slopeRat, clpsFlag, failureFlag, endTime, stage);
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
                    if (stage.CompareTo("Hunt") == 0 && clpsFlag == 1)
                    {
                        stage = "Bracket";
                        nFill = nPnt - 1;

                    }
                    if (stage.CompareTo("Bracket") == 0)
                    {
                        var clpsRsltn = (saC - saNC) / saNC;
                        if (clpsRsltn < 0.05)
                        {
                            stage = "Fill";
                        }
                    }
                    if (stage.CompareTo("Hunt") == 0)
                    {
                        sa = sa + SaStep * (nPnt + 1);
                    }
                    else if (stage.CompareTo("Bracket") == 0)
                    {
                        sa = saNC + (saC - saNC) / 3;
                    } else {
                        if (nFill == 0 || !GoFill || nPnt > MaxNumPoints)
                        {
                            break;
                        }
                        sa = (saVec[nFill] + saVec[nFill - 1]) / 2;
                        nFill--;
                    }
                }
                SortArray(ref lineVec, ref saVec);

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

            label5.Text = "Finished";
        }

        private void SortArray(ref List<string> lineVec, ref List<double> saVec)
        {
            for (var i = 0; i < saVec.Count-1; i++)
            {
                var min = saVec[i];
                var minInd = i;
                for (var j = i; j < saVec.Count; j++)
                {
                    if (saVec[j] < min)
                    {
                        min = saVec[j];
                        minInd = j;
                    }
                }
                var tmp = saVec[i];
                saVec[i] = saVec[minInd];
                saVec[minInd] = tmp;
                var tmp2 = lineVec[i];
                lineVec[i] = lineVec[minInd];
                lineVec[minInd] = tmp2;
            }
        }

        private bool RunOps(int iRec, double sa, string workingDir, string fileName, ref int flag, ref double disp, ref double endTime)
        {
            var file = string.Format(CultureInfo.InvariantCulture, "{2}/run_{0}_{1}.tcl", iRec, sa, workingDir);
            var list = new List<string>
                    {
                        string.Format(CultureInfo.InvariantCulture, "set iRec {0}", iRec),
                        string.Format(CultureInfo.InvariantCulture, "set sa {0}", sa),
                            $"source {fileName}",
                            "set file [open tmpOut-$iRec-$sa.txt w]",
                            "puts $file \"$failureFlag $endTime $disp\"",
                            "close $file"
                    };
            var writer = File.CreateText(file);
            foreach (var l in list)
            {
                writer.WriteLine(l);
            }
            writer.Close();
            var file0 = string.Format(CultureInfo.InvariantCulture, "\"{0}\"", file);
            Console.Title = file0 + "(www.CivilSoftScience.com)";
            var info = new ProcessStartInfo(OpsExePath, file0)
            {
                WorkingDirectory = workingDir,
                //RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };
            Process theProcess = null;
            try
            {
                theProcess = new Process { StartInfo = info };
            }
            catch
            {
                Console.WriteLine($"ERROR: The OpenSees.exe file was not found in {OpsExePath}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(false);
                return false;
            }
            //theProcess.ErrorDataReceived += NetErrorDataHandler;
            theProcess.OutputDataReceived += NetStndrdDataHandler;
            var watch = Stopwatch.StartNew();
            var strt = DateTime.Now;
            //Console.WriteLine("Analysis started at: {0}", strt);
            Console.ForegroundColor = ConsoleColor.White;
            try
            {
                theProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR Running OpenSees.exe: {ex.Message}");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(false);
                return false;
            }
            //theProcess.BeginErrorReadLine();
            theProcess.BeginOutputReadLine();
            theProcess.WaitForExit();
            watch.Stop();
            var processTime = watch.Elapsed;
            Console.WriteLine("-------------------------------------------------------------------------------------------------------");
            Console.WriteLine("End of script {0} reached.", file0);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Analysis started at: {0}", strt);
            Console.WriteLine("Analysis ended at: {0}", DateTime.Now);
            Console.WriteLine("Process time = {0}", watch.Elapsed);
            var file2 = string.Format(CultureInfo.InvariantCulture, "{0}/tmpOut-{1}-{2}.txt", workingDir, iRec, sa);
            var line = File.ReadAllLines(file2)[0];
            var words = line.Split(' ');
            try
            {
                flag = Convert.ToInt16(words[0], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
               Console.WriteLine($"Error interpreting flag value: {words[0]}");
                return false;
            }
            try
            {
                endTime = Convert.ToDouble(words[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine($"Error interpreting endTime value: {words[1]}");
                return false;
            }
            try
            {
                disp = Convert.ToDouble(words[2], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                Console.WriteLine($"Error interpreting disp value: {words[2]}");
                return false;
            }
            File.Delete(file);
            File.Delete(file2);
            return true;
        }

        private static void NetStndrdDataHandler(object sendingProcess,
                        DataReceivedEventArgs errLine)
        {
            Console.WriteLine(errLine.Data);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            startTrace();
        }
    }
}
