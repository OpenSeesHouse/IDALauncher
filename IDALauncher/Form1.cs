using Microsoft.Win32;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDALauncher
{
    public partial class Form1 : Form
    {
        public string OpsExePath = @"C:\Program Files\OpenSees-CSS\OpenSees.exe";
        public string TclModelPath = "";
        public int NumThreads;
        List<int> recList;

        List<Thread> myThreads = new List<Thread>();
        public Form1()
        {
            InitializeComponent();
            RegistryKey rk = Registry.CurrentUser;
            String key = "SOFTWARE\\CivilSoftScience\\IDALauncher\\R1.0";
            RegistryKey sk = rk.CreateSubKey(key);
            OpsExePath = (string)sk.GetValue("EXEPATH");
            if (OpsExePath == null)
            {
                OpsExePath = @"C:\Program Files\OpenSees-CSS\OpenSees.exe";
                sk.SetValue("EXEPATH", @"C:\Program Files\OpenSees-CSS\OpenSees.exe");
            }
            OpsPathTB.Text = OpsExePath;
        }

        private void LaunchBtn_Click(object sender, EventArgs e)
        {
            try
            {
                NumThreads = Convert.ToInt16(NumThreadsTB.Text);
            } catch
            {
                MessageBox.Show($"Unrecognized Threads Number Specified: {NumThreadsTB.Text}", "Error", MessageBoxButtons.OK);
                return;
            }
            if (TclModelPath == "")
            {
                if (TCLPathTB.Text == "")
                {
                    MessageBox.Show("Please Select the TCL Model file for dynamic analysis of your model", "Error", MessageBoxButtons.OK);
                    return;
                }
                TclModelPath = TCLPathTB.Text;
            }
            string[] argv = new string[5];
            argv[0] = textBox3.Text;
            argv[1] = textBox4.Text;
            var args = recRangeBox.Text;
            if (args.Length == 0)
            {
                MessageBox.Show("Please Enter Record Range", "Error", MessageBoxButtons.OK);
                return;
            }
            while (args.Contains(" "))
                args = args.Replace(" ", "");
            //var args2 = args.Replace(",", "_");
            var recs = args.Split(',');
            recList = new List<int>();
            foreach (var recStr in recs)
            {
                int val1, val2;
                var ind2 = recStr.IndexOf('-');
                if (ind2 == -1)
                {
                    try
                    {
                        val1 = Convert.ToInt16(recStr, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch
                    {
                        MessageBox.Show("Error converting string to int", "Error", MessageBoxButtons.OK);
                        return;
                    }
                    recList.Add(val1);
                    //newRec.AppendFormat("{0},", recStr);
                    continue;
                }
                var str1 = recStr.Substring(0, ind2);
                ind2++;
                var str2 = recStr.Substring(ind2);
                try
                {
                    val1 = Convert.ToInt16(str1, System.Globalization.CultureInfo.InvariantCulture);
                    val2 = Convert.ToInt16(str2, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    MessageBox.Show("Error converting string to int", "Error", MessageBoxButtons.OK);
                    return;
                }
                if (val1 >= val2)
                {
                    MessageBox.Show("Error interpreting range values", "Error", MessageBoxButtons.OK);
                    return;
                }
                for (; val1 <= val2; val1++)
                {
                    recList.Add(val1);
                }
            }
            if (HuntFillRadio.Checked && NumThreads == 1 && FullRB.Checked)
            {
                var folder = Directory.GetParent(Application.ExecutablePath).FullName;
                var exePath = $"{folder}/FullSingleThreadIDA.exe";
                var tmpFilename = $"{folder}/IDALauncher.inp.tmp";
                var tmpFile = File.CreateText(tmpFilename);
                argv[2] = textBox1.Text;
                argv[3] = textBox2.Text;
                argv[4] = GoFillCB.Checked ? "1" : "0";
                foreach (var str in argv)
                    tmpFile.WriteLine(str);

                args = "";
                foreach (var rec in recList)
                    args += $"{rec},";
                tmpFile.WriteLine(args);
                tmpFile.WriteLine(TclModelPath);
                tmpFile.WriteLine(OpsExePath);
                tmpFile.Close();
                if (! File.Exists(exePath))
                    MessageBox.Show($"ERROR: The FullSingleThreadIDA.exe file was not found");

                var info = new ProcessStartInfo(exePath, tmpFilename);
                var wdir = Directory.GetParent(TclModelPath).FullName;
                info.WorkingDirectory = wdir;
                Process theProcess = null;
                try
                {
                    theProcess = new Process { StartInfo = info };
                }
                catch
                {
                    MessageBox.Show($"ERROR: The HuntFillSequential.exe file was not found");
                }
                try
                {
                    theProcess.Start();
                }
                catch
                {
                    MessageBox.Show($"ERROR: The HuntFillSequential.exe file was not found");
                }
            }
            else
            {
                MessageBox.Show("This combination of options is not supported yet", "Sorry", MessageBoxButtons.OK);
                return;
            }

        }
        
        private void BrowsExeBtn_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.AddExtension = false;
            dlg.CheckFileExists = true;
            dlg.DefaultExt = "exe";
            dlg.Multiselect = false;
            dlg.Title = "Select the OpenSees.exe file";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            OpsExePath = dlg.FileName;
            OpsPathTB.Text = OpsExePath;
            RegistryKey rk = Registry.CurrentUser;
            String key = "SOFTWARE\\CivilSoftScience\\IDALauncher\\R1.0";
            RegistryKey sk = rk.CreateSubKey(key);
            sk.SetValue("EXEPATH", OpsExePath);
        }

        private void BrowseTCLBtn_Click(object sender, EventArgs e)
        {
            TclModelPath = TCLPathTB.Text = "";

            var dlg = new OpenFileDialog();
            dlg.AddExtension = false;
            dlg.CheckFileExists = true;
            dlg.DefaultExt = "tcl";
            dlg.Multiselect = false;
            dlg.Title = "Select the OpenSees.exe file";
            if (dlg.ShowDialog() != DialogResult.OK)
                return;
            TclModelPath = TCLPathTB.Text =  dlg.FileName;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (var th in myThreads)
                th.Abort();
        }
    }

}
