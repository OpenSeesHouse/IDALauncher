using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IDAServer
{
    public class IdaClient
    {
        private int NDigitsSize = 5;
        public string Name { set; get; }
        public IPAddress Ip { set; get; }
        public int NumCores { set; get; }
        public int WorkingCores { get; set; }
        public bool IsRegistered = false;
        public NetworkStream TheStream;
        public Message TheMessage { set; get; }
        public List<SingleRunJob> JobList = new List<SingleRunJob>();
        public List<ClientActivityLog> ActivityLogList;
        private TcpClient MyTcpClient;
        public IdaClient(TcpClient myTcpClient)
        {
            MyTcpClient = myTcpClient;
        }
        public void SendMessage(string message)
        {
            if (!MyTcpClient.Connected)
                throw new DisconnectExcption();
            message = string.Format("{0}-{1}", message.Length, message);
            byte[] msg = Encoding.ASCII.GetBytes(message);
            TheStream.Write(msg, 0, msg.Length);
        }

        public Message RecieveMessage()
        {
            TheMessage = new Message("");
            var size = ReadSize();
            if (size == 0)
                return null;
            var sz = size - TheMessage.Text.Length;
            if ( sz > 0 )
            {
                ReadMsg(sz);
            }
            TheMessage.UpdateSelf();
            return TheMessage;
        }

        private int ReadSize()
        {
            if (!MyTcpClient.Connected)
                throw new DisconnectExcption();
            if (!TheStream.DataAvailable)
                return 0;
            var msg = new byte[NDigitsSize];
            TheStream.Read(msg, 0, msg.Length);
            TheMessage.Text = Encoding.UTF8.GetString(msg);
            var ind = TheMessage.Text.IndexOf("\0", StringComparison.Ordinal);
            if (ind != -1)
                TheMessage.Text = TheMessage.Text.Remove(ind);
            var str = "";
            ind = TheMessage.Text.IndexOf("-", StringComparison.Ordinal);
            if (ind == -1)
            {
                str = TheMessage.Text;
                TheMessage.Text = "";
            }
            else
            {
                str = TheMessage.Text.Substring(0, ind);
                TheMessage.Text = TheMessage.Text.Substring(ind + 1);
            }
            var size = 1000;
            try
            {
                size = Convert.ToInt16(str);
            }
            catch
            {
                //ignored
            }
            return size;
        }

        internal NetworkStream GetStream()
        {
            return MyTcpClient.GetStream();
        }

        private void ReadMsg(int size)
        {
            if (!MyTcpClient.Connected)
                throw new DisconnectExcption();
            if (!TheStream.DataAvailable)
            {
                TheMessage.Text = "";
                return;
            }
            var msg = new byte[size];
            TheStream.Read(msg, 0, msg.Length);
            TheMessage.Text += Encoding.UTF8.GetString(msg);
            var ind = TheMessage.Text.IndexOf("\0", StringComparison.Ordinal);
            if (ind != -1)
                TheMessage.Text = TheMessage.Text.Remove(ind);
        }
    }

    public class DisconnectExcption : Exception
    {
        public DisconnectExcption(): base("Client is Offline")
        {
        }
    }

    public class ClientActivityLog
    {
        DateTime IssueDate { set; get; }
        Message.ActionType IssuedAction { set; get; }
    }

}
