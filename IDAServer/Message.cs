using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDAServer
{
    public class Message
    {
        public Dictionary<char, ActionType> ActionKeyDic = new Dictionary<char, ActionType>
            {
                {'R', ActionType.Register},
                {'Q', ActionType.JobRequest},
                {'A', ActionType.JobAbort},
                {'S', ActionType.ResultSet}
            };
        
        public enum ActionType
        {
            Register, JobRequest, ResultSet, JobAbort
        }

        public SingleRunJob JobRef = null;

        public void UpdateSelf()
        {
            if (string.IsNullOrEmpty(Text))
                return;
            var ch = Text.Substring(0,1)[0];
            if (!ActionKeyDic.ContainsKey(ch))
                throw new Exception("Message Action Char not Recognized");

            Action = ActionKeyDic[ch];
            Text = Text.Substring(1);
        }
        public ActionType Action { set; get; }

        public string Text { set; get; }

        public Message(string msgStr)
        {
            Text = msgStr;
            UpdateSelf();
        }
    }
}
