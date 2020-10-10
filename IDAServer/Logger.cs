using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Configuration;
using System.Threading;
using System.Windows.Forms;

namespace IDAServer
{
    public static class Logger
    {
        public static string LogFile { set; get; }
        public static Server MyForm { set; get; }

        public static void Start()
        {
            Log("-------------------------------------------------------------------");
            Log("Server Started:");
            Log(DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public static void Log(string msg)
        {
            if (!string.IsNullOrEmpty(LogFile))
                File.AppendAllText(LogFile, msg + Environment.NewLine);
        }
        public static void Status(string msg)
        {
            Log(msg+"\r\n");
            if (MyForm != null)
                MyForm.AddStatusMsg(msg);
        }

        public static void Error(string msg)
        {
            Log(msg + "\r\n");
            if (MyForm != null)
                MyForm.ShowError(msg);
            Thread.CurrentThread.Abort();
        }

        public static void Close()
        {
            Log("--------------------------------------- END OF SESSION -------------------------------------");
        }

        internal static void LogConnected(string name, IPAddress ip, int port)
        {
            //Log(string.Format("Successfully connected to client {0} with ip {1} at port {2}", name, ip, port));
        }

        internal static void LogJobSent(SingleRunJob job, IdaClient idaClient)
        {
            //Log(string.Format("New job (model= {0}, record= {1}, IM= {2}) was sent to {3} computer", job.Model, job.Record, job.Im, idaClient.Name));
        }

        internal static void LogPostingFinished()
        {
            //Log("Posting the jobs finished");
        }

        public static void LogResultRecieved(SingleRunJob job, IdaPoint pnt)
        {
            //Log(string.Format("New result (model= {0}, record= {1}, IM= {2})", job.Model, job.Record, job.Im));
        }

        public static void LogIdaFinished(IdaJob idaJob)
        {
            //Log("IDA (model= {idaJob.Model}, record= {idaJob.Record}) Finished");
        }

        public static void LogNumCoresChanged(IdaClient idaClient)
        {
            //Log("Client {IdaClient.Name} is running with {IdaClient.NCores} cores");
        }

        public static void LogConnectioFailed(IdaClient data)
        {
            //Log("failed to connect to {data.Name} computer");
        }

        public static void UpdateForClients()
        {
            MyForm.UpdateClientTable();
        }
    }
}
