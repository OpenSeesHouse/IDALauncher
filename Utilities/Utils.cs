using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace Utilities
{
    public static class Utils
    {
        public static void initiateConsole()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\n\n");
            Console.WriteLine("  ******************************* OpenSeesLauncher *******************************");
            Console.WriteLine("  *                       Developed at Civil Soft Science                        *");
            Console.WriteLine("  *             www.CivilSoftScience.com,  CivilSoftScience@gmail.com            *");
            Console.WriteLine("  ********************************************************************************\n");

        }
        public static void SortArray(ref List<string> lineVec, ref List<double> saVec)
        {
            for (var i = 0; i < saVec.Count - 1; i++)
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
        public static bool RunOps(int iRec, double sa, string workingDir, string OpsExePath, string tclModelName, ref int flag, ref double disp, ref double endTime, ref TimeSpan processTime, ref string msg, bool hideConsol = false)
        {
            msg = "Successful";
            var file = string.Format(CultureInfo.InvariantCulture, "{2}/run_{0}_{1}.tcl", iRec, sa, workingDir);
            var tmpFile = Path.GetRandomFileName();
            var list = new List<string>
                    {
                        string.Format(CultureInfo.InvariantCulture, "set iRec {0}", iRec),
                        string.Format(CultureInfo.InvariantCulture, "set sa {0}", sa),
                            $"source {tclModelName}",
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
                UseShellExecute = false,
                CreateNoWindow = hideConsol
            };
            Process theProcess = null;
            try
            {
                theProcess = new Process { StartInfo = info };
            }
            catch
            {
                masg = 
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
            processTime = watch.Elapsed;
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

        public static void NetStndrdDataHandler(object sendingProcess,
                        DataReceivedEventArgs errLine)
        {
            Console.WriteLine(errLine.Data);
        }
    }
}
