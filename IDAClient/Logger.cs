using System;
using System.Globalization;
using System.IO;

namespace IDAClient
{
    public static class Logger
    {
        public static string LogFile { set; get; }
        public static ClientForm MyForm { set; get; }

        public static void Start(string type)
        {
            MyType = type;
            //Log("\n");
            
            //Log("-------------------------------------------------------------------");
            Log(MyType + " Started:" );
            Log(DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\n");
            //Log("____________________________________________________________\n");
        }

        public static string MyType { get; set; }

        public static void Log(string msg)
        {
            if (!string.IsNullOrEmpty(LogFile))
                File.AppendAllText(LogFile, msg);
            //Console.WriteLine(msg);
            if ( MyForm != null)
                MyForm.AddMsgToBox(msg + "\n");
            //MyForm?.AddMsgToBox($"\n{msg}");
        }


        public static void LogNewJobStarted(ClientCore core)
        {
            var model = core.MyJob.ModelFullPath;
            var rec = core.MyJob.Record;
            var im = core.MyJob.Im;
            MyForm.UpdateJobInTable(model, rec, im, "Running", core.CoreTag.ToString(), core.MyJob.StartTime, "---", "---");
        }

        public static void Close()
        {
            if (!string.IsNullOrEmpty(LogFile))
                File.AppendAllText(LogFile, "--------------------------------------- END OF SESSION -------------------------------------\n\n");
        }

        public static void LogJobFinished(ClientCore core)
        {
            var model = core.MyJob.ModelFullPath;
            var rec = core.MyJob.Record;
            var im = core.MyJob.Im;
            MyForm.UpdateJobInTable(model, rec, im, "Finished", core.CoreTag.ToString(), core.MyJob.StartTime, core.MyJob.EndTime, core.MyJob.JobTime);
        }
        
        public static void LogNworkersChanged()
        {
            
            MyForm.ChangeFormTitle(true, ProcessManager.NWorkers);
        }

        public static void LogConctnStatChanged(bool cnctd)
        {
            MyForm.ChangeFormTitle(cnctd, ProcessManager.NWorkers);
        }
    }
}
