using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDAServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Logger.LogFile = "";
            Logger.TheTextBox = null;
            //: writes only to Console

            Logger.Start();

            Server.NumAllRecs = 44;
            Server.WorkingDir = "WorkingDir"; // is located in bin\Debug
            Server.ProjectFile = "Project.txt"; // is located in WorkingDir
            Server.MachinesFile = "Machines.txt";
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
    }
}
