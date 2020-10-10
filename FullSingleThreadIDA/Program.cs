using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDALauncher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        /*static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }*/
        static void Main(string[] tmpFile)
        {
            string[] argv = File.ReadAllLines(tmpFile[0]);
            File.Delete(tmpFile[0]);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            MessageBox.Show("Pause", "attach to process", MessageBoxButtons.OK);
            double saStart, saStep, saStepIncr;
            int numPnts;
            bool goFill;
            try
            {
                saStart = Convert.ToDouble(argv[0], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Unacceptable saStart", "Error", MessageBoxButtons.OK);
                return;

            }
            try
            {
                saStep = Convert.ToDouble(argv[1], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Unacceptable saStep", "Error", MessageBoxButtons.OK);
                return;

            }
            try
            {
                saStepIncr = Convert.ToDouble(argv[2], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Unacceptable saStepIncr", "Error", MessageBoxButtons.OK);
                return;

            }
            var recStrs = argv[3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<int> recordList;
            recordList = new List<int>(recStrs.Length);
            foreach (var recStr in recStrs)
            {
                try
                {
                    recordList.Add(Convert.ToInt16(recStr, System.Globalization.CultureInfo.InvariantCulture));
                }
                catch
                {
                    MessageBox.Show($"Unacceptable recStr: {recStr}", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            try
            {
                numPnts = Convert.ToInt16(argv[4], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                MessageBox.Show("Unacceptable numPnts", "Error", MessageBoxButtons.OK);
                return;
            }
            switch (argv[5])
            {
                case "0":
                    goFill = false;
                    break;
                case "1":
                    goFill = true;
                    break;
                default:
                    MessageBox.Show("Unacceptable numPnts", "Error", MessageBoxButtons.OK);
                    return;
            }
            var theForm = new FullSTForm(saStart, saStep, saStepIncr, numPnts, goFill, recordList, argv[6], argv[7]);

            /*var tclFile = @"E:\Research\Dr Mansouri\Damper paper\new models\MRF-4\runNTH.tcl";
            var theForm = new Form1(0.05, 0.05, 0.05, 20, true, new List<int>() { 1 }, tclFile, @"C:\Users\User\Desktop\tmp\OpenSees.exe");*/


            Application.Run(theForm);
        }
    }
}
