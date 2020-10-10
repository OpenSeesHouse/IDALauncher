using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IDAClient
{
    public class ClientManager
    {
        public static IPAddress ServerIp;
        public static int Port = 8111;
        public static IPAddress MyIp;
        public static string MyName { get; set; }
        public static TcpClient Server;
        public static NetworkStream ServerStream;
        //public static int CoreCount ;
        public static ProcessManager ProcManager;
        public static string ExecPath { get; set; }
        public static string WorkingDir { get; set; }
        public static string TclModelName { get; set; }
        public static int ServerCnctWaitSecond = 1;
        public int MaxCpuUsage { get; set; }
        public ClientManager(string folder, string opsExePath, string tclModelName, string serverIp, int maxCpuUsage)
        {
            ServerIp = IPAddress.Parse(serverIp);
            //CoreCount = Environment.ProcessorCount;
            //CoreCount = 2;
            WorkingDir = folder;
            MyName = Dns.GetHostName();
            MyIp = LocalIpAddress();
            MaxCpuUsage = maxCpuUsage;
            TclModelName = tclModelName;
            ExecPath = opsExePath;
            //Logger.MakeCoreTable(CoreCount);

        }


        private static IPAddress LocalIpAddress()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                return IPAddress.Parse("127.0.0.1");
            }

            var host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        public void Manage()
        {
            Logger.Start("Client");
            MakeConnection();
            ProcManager = new ProcessManager(WorkingDir, ExecPath, TclModelName, MaxCpuUsage);
            ProcManager.StartHandling();
            EndManaging();
        }



        public static void MakeConnection()
        {
            Logger.LogConctnStatChanged(false);
            while (true)
            {

                try
                {
                    Server = new TcpClient(ServerIp.ToString(), Port);
                    break;
                }
                catch
                {
                    Thread.Sleep(ServerCnctWaitSecond*1000);
                }
            }
            //Server = new TcpClient(ServerIp.ToString(), Port);
            Logger.LogConctnStatChanged(true);
            Logger.Log("Server Connection Done");
            Logger.Log("Server IP : "+ ServerIp + "\n");
            ServerStream = Server.GetStream();
            var msg = RecieveMessage(0, "");
            if (String.CompareOrdinal(msg, "Start") == 0)
            {
                SendMessage($"{MyIp} {MyName} {Environment.ProcessorCount}");
            }
        }

        public static bool SendMessageSize(string message)
        {
            var msg = Encoding.ASCII.GetBytes(message);
            while (true)
            {
                try
                {
                    ServerStream.Write(msg, 0, msg.Length);
                    break;
                }
                catch
                {
                    Logger.Log("Serever Disconneted");
                    Thread.Sleep(ServerCnctWaitSecond*1000);
                    MakeConnection();
                    Logger.Log("Server Connected");
                }
            }
            return true;
        }

        public static bool SendMessage(string message)
        {
            SendMessageSize($"{message.Length}-1");
            var msg = Encoding.ASCII.GetBytes(message);
            while (true)
            {
                try
                {
                    ServerStream.Write(msg, 0, msg.Length);
                    break;
                }
                catch
                {
                    Logger.Log("Serever Disconneted");
                    Thread.Sleep(ServerCnctWaitSecond*1000);
                    MakeConnection();
                }
            }
            return true;
        }

        public static string RecieveMessage(int size, string msgToResend)
        {
            var isSize = false;
            if (size == 0)
            {
                size = 5;
                isSize = true;
            }
            var msg = new byte[size];
            string str;
            var str1 = "";
            while (true)
            {
                try
                {
                    ServerStream.Read(msg, 0, size);
                    str = Encoding.UTF8.GetString(msg);
                    var ind = str.IndexOf("\0", StringComparison.Ordinal);
                    if (ind != -1)
                        str = str.Remove(ind);
                    if (isSize)
                    {
                        ind = str.IndexOf("-1", StringComparison.Ordinal);
                        if (ind == -1)
                            ind = str.Length;
                        else
                            str1 = str.Substring(ind + 2);
                        size = Convert.ToInt16(str.Substring(0, ind));
                    }
                    break;
                }
                catch
                {
                    Logger.Log("Serever Disconneted");
                    Thread.Sleep(ServerCnctWaitSecond*1000);
                    MakeConnection();
                    SendMessage(msgToResend);
                }
            }
            if (isSize)
            {
                str = RecieveMessage(size, msgToResend);
                str = str1 + str;
            }
            return str;
        }

        public void EndManaging()
        {
            Logger.Close();
        }
    }
}
