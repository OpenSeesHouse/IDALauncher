using System;
using System.Collections.Generic;
using System.Linq;

namespace IDAServer
{
    public static class JobPool
    {
        public static List<SingleRunJob> JobList = new List<SingleRunJob>();

        public static void AddJob(SingleRunJob job)
        {
            var i = JobList.TakeWhile(oldJob => job.Priority >= oldJob.Priority).Count();
            JobList.Insert(i, job);
        }

        public static void CancelJobs(IdaJob owner)
        {
            var n = 0;
            foreach (var job in JobList.Where(job => (job.MyIdaJob == owner)))
            {
                if (job.TheStatus == SingleRunJob.Status.Waiting)
                    n++;
                job.TheStatus = SingleRunJob.Status.Cancelled;
            }
            AddJobs(owner.GetNextJobs(n));
            //for (var i = 0; i < JobList.Count; i++)
            //{
            //    JobList[i].TheStatus = JobList[i].MyIdaJob == owner
            //        ? SingleRunJob.Status.Cancelled
            //        : JobList[i].TheStatus;
            //}
        }

        public static void AddJobs(List<SingleRunJob> jobList)
        {
            foreach (var job in jobList)
            {
                if (job != null)
                {
                    job.TheStatus = SingleRunJob.Status.Waiting;
                    AddJob(job);
                }
            }
        }

        public static SingleRunJob GetJob()
        {
            var ind = 0;
            for (var i = 0; i < JobList.Count; i++)
            {
                if (JobList[i].TheStatus == SingleRunJob.Status.Waiting)
                    break;
                ind++;
            }
            if (ind == JobList.Count)
                return null;
            var job = JobList[ind];
            job.TheStatus = SingleRunJob.Status.Posted;
            job.MyIdaJob.OnJobsSent();
            var newJobList = job.MyIdaJob.GetNextJobs(1);
            var newJob = newJobList[0];
            if (newJob != null)
                AddJob(newJob);
            return job;
        }

        public static SingleRunJob GetJob(string model, string rec, double im)
        {
            var theJob = JobList.Where(job => (job.Model == model && job.Record == rec && Math.Abs(job.Im - im) < 1e-3)).ElementAt(0);
            return theJob;
        }
    }
}
