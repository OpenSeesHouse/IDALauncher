using System;

namespace IDAClient
{
    public class Job
    {
        public string ModelFullPath  { set; get; }
        public int Record  { set; get; }
        public double Im  { set; get; }
        public string StartTime { set; get; }
        public string EndTime { set; get; }
        public string JobTime { set; get; }

        public Job(string modelPath, int record, double im)
        {
            ModelFullPath = modelPath;
            Record = record;
            Im = im;
        }

    }
}
