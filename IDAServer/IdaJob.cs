using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace IDAServer
{
    public class IdaJob : Job
    {
        public int NWorkers { get; set; }
        public IdaStatus Status { get; set; }
        public List<IdaPoint> Graph { get; set; }
        public int RunningJobs { get; set; }
        public int CompleteJobs { get; set; }
        private readonly ParallelTracer _theTracer;

        public IdaJob(string folder, string model, string rec, ParallelTracer tracer)
            : base(folder, model, rec)
        {
            _theTracer = tracer.GetCopy();
            NWorkers = 1;
        }


        public void Start()
        {
            var dir = string.Format("{0}{3}/{1}/{2}", Program.TheServer.WorkingDir, Model, Record,
                !string.IsNullOrEmpty(SubFolder) ? "/" + SubFolder : "");
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString() + "\nFailed To Create Directory for Record: " + Record);
                Thread.CurrentThread.Abort();
            }
            var outFile = string.Format("{0}/IDA.txt", dir);
            _theTracer.StartTracing(outFile);
        }

        public List<SingleRunJob> GetNextJobs(int num)
        {
            List<double> toPostList = _theTracer.GetNextImsToPost(num);
            List<int> prioriList = _theTracer.GetNextPriosToPost(num, NWorkers);
            var jobs = new List<SingleRunJob>(num);
            int i;
            for (i = 0; i < num; i++)
                jobs.Add(null);
            i = 0;
            foreach (var im in toPostList)
            {
                if (Math.Abs(im) < 1e-6)
                    break;
                var job = new SingleRunJob(SubFolder, Model, Record, im, prioriList[i]) {MyIdaJob = this};
                jobs[i] = job;
                i++;
            }
            return jobs;
        }

        public void SetResult(SingleRunJob job, IdaPoint resPnt, out bool cancelPool, out bool deleteMe)
        {
            NWorkers--;
            _theTracer.UpdateStatus(ref resPnt, job.TheStatus);
            deleteMe = false;
            cancelPool = false;
            if (NWorkers <= 1)
            {
                deleteMe = true;
                _theTracer.EndTracing();
                return;
            }
            if (_theTracer.StageChanged)
            {
                //we must delete posted jobs before execution
                cancelPool = true;
            }
        }

        public void OnJobsSent()
        {
            NWorkers ++;
        }

    }

    public enum IdaStatus {Waiting, Running, Complete}
}
