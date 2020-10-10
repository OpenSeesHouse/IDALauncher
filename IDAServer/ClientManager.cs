using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IDAServer
{
    public static class ClientManager
    {
        public static long ListenSeconds = 4;
        public static int RetrySeconds = 1;
        public static int Port = 8111;
        public static List<IdaClient> ClientList = new List<IdaClient>();

        public static bool ListenToAny()
        {
            // start listening to connecting clients for a ListenSeconds time
            var watch = Stopwatch.StartNew();
            //Logger.Status("Waiting for clients ...");
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            while (watch.ElapsedMilliseconds < ListenSeconds*1000)
            {
                while (listener.Pending())
                {
                    IdaClient client = new IdaClient(listener.AcceptTcpClient());
                    RegisterClient(client);
                    listener.Stop();
                    listener = new TcpListener(IPAddress.Any, Port);
                    listener.Start();
                }
                Thread.Sleep(RetrySeconds*1000);
            }
            listener.Stop();
            if (ClientList.Count == 0)
            {
                Logger.Status("failed to connect to clients");
                return false;
            }
            Logger.Status(string.Format("Successfully connected to {0} clients", ClientList.Count));
            return true;
        }
        
        public static void RegisterClient(IdaClient client)
        {
            client.IsRegistered = true;
            ClientList.Add(client);
            client.TheStream = client.GetStream();
            client.RecieveMessage();
            HandleClientMessage(client);
        }

        public static List<IdaClient> GetRgstrdList()
        {
            return ClientList.Where(client => client.IsRegistered).ToList();
        }

        private static void HandleClientMessage(IdaClient client)
        {
            switch (client.TheMessage.Action)
            {
                case Message.ActionType.JobAbort:
                    //client.JobList.
                    break;
                case Message.ActionType.JobRequest:
                    break;
                case Message.ActionType.Register:
                    var list = client.TheMessage.Text.Split(' ').ToList();
                    client.Ip = IPAddress.Parse(list[0]);
                    client.Name = list[1];
                    client.NumCores = Convert.ToInt16(list[2]);
                    Logger.UpdateForClients();
                    break;
                case Message.ActionType.ResultSet:
                    break;
            }
        }

        public static void UnRegister(IdaClient client)
        {
            client.IsRegistered = false;
        }

        public static void StartHandling()
        {
            var done = false;
            while (!done)
            {
                foreach (var data in ClientList)
                {
                    var clientData = data;
                    var msg = data.RecieveMessage();
                    if (msg == null)
                        continue;
                    if (msg.Text.Contains("Request"))
                    {
                        var job = JobPool.GetJob();
                        if (job != null)
                        {
                            data.SendMessage(job.ToString());
                            Logger.LogJobSent(job, clientData);
                        }
                        else
                        {
                            data.SendMessage("Done");
                            Logger.LogPostingFinished();
                        }
                        var ind = msg.Text.IndexOf("Request", StringComparison.Ordinal);
                        var coreStr = msg.Text.Substring(0, ind);
                        try
                        {
                            int nCore = Convert.ToInt16(coreStr);
                            if (clientData.NumCores != nCore)
                            {
                                clientData.NumCores = nCore;
                                Logger.LogNumCoresChanged(clientData);
                            }
                        }
                        catch 
                        {
                            // ignored
                        }
                        msg.Text = msg.Text.Remove(0, ind+7);
                    }
                    if (string.IsNullOrEmpty(msg.Text)) continue;
                    SingleRunJob thejob;
                    IdaPoint pnt;
                    done = JobManager.SetResultString(msg.Text, out thejob, out pnt);
                    Logger.LogResultRecieved(thejob, pnt);
                }
            }

        }

        public static IdaClient GetClient(IPAddress ip)
        {
            var list = ClientList.Where(ida => ida.Ip.Equals(ip)).ToList();
            return list.Count == 0 ? null : list[0];
        }
    }
}
