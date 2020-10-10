using System;

namespace IDAServer
{
    public abstract class Job
    {
        public string SubFolder { get; set; }
        public string Model { set; get; }
        public string Record { set; get; }
        public int UniqueId { set; get; }
        private static int _lastId = 0;
        protected Job(string folder, string model, string rec)
        {
            UniqueId = ++_lastId;
            SubFolder = string.CompareOrdinal(folder, "-") == 0 ? String.Empty : folder;
            Model = model;
            Record = rec;
        }

    }
}
