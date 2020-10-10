using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Utilities;

namespace IDAClient
{
    public class ClientCore
    {
        public static string OpsFile { set; get; }
        public static int WriteWaitSeconds = 1;
        public int CoreTag { get; set; }
        public bool HasResult ;
        public Job MyJob { get; set; }
        public Result MyResult { get; set; }
        public Thread MyThread;
        //public ThreadState LastState = ThreadState.Standby;
        //public Process CurProcessor = new Process();
        //private List<string> _resList;

        public ClientCore(int num, string clientAddress, string opsFile, string tclModel)
        {
            CoreTag = num;
            MyAddress = clientAddress;
            MyThread = new Thread(Execute);
            OpsFile = opsFile;
            TclModel = tclModel;
            //MyThread = new Thread(Execute);
            //LastState = ThreadState.Unstarted;
            HasResult = false;
        }

        public static void Execute(object obj)
        {
            var client = (ClientCore)obj;
            var workingDir = Directory.GetParent(client.MyJob.ModelFullPath).FullName;
            var tclModel = Path.GetFileName(client.MyJob.ModelFullPath);
            int flag = 0;
            double edp = 0;
            double thaEndTime = 0;
            Utils.RunOps(client.MyJob.Record, client.MyJob.Im, workingDir, OpsFile, tclModel, ref flag, ref edp, ref thaEndTime);
            List<string> resList;
            try
            {
                 resList = File.ReadAllText(tmpOutFile).Split('\n', '\r').ToList();
            }
            catch
            {
                Logger.Log($"Analysis output not generated for job at core {client.CoreTag}");
                return;
            }
            // clean-up resList:
            var ind = 0;
            while (ind < resList.Count)
            {
                if (resList[ind].Length == 0)
                    resList.RemoveAt(ind);
                else
                    ind++;
            }
            //teta = lines[0];
            //endT = lines[1];
            //dvrgFlg = lines[2];
            resList.Add(processTime.ToString());
            client.MyResult = new Result(client.MyJob, resList );
            client.HasResult = true;
        }


        public String GetResult()
        {
            return MyResult.GetMassage();
        }
        

        public void Start(Job job)
        {
            MyJob = job;
            var myTime = DateTime.Now;
            MyJob.JobTime = myTime.Hour + ":" + myTime.Minute + ":" + myTime.Second + " " + myTime.ToString("tt", CultureInfo.InvariantCulture);
            //LastState = ThreadState.Unstarted;
            if (MyThread.IsAlive)
                MyThread.Start(this);
            else
            {
                MyThread = new Thread(Execute);
                MyThread.Start(this);
            }
        }
    }
}
