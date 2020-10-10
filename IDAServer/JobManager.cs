using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace IDAServer
{
    public static class JobManager
    {

        public static List<IdaJob> IdaJobPool = new List<IdaJob>();
        public static List<IdaJob> LoadedJobsPool = new List<IdaJob>();
        public static int MaxPostedIms { set; get; }
        public static int MaxActiveIdas { set; get; }

        static JobManager()
        {
            try
            {
                MaxPostedIms = Convert.ToInt16(Program.TheServer.NumPostsBox.Text);
            }
            catch
            {
                Logger.Error("Error reading Num. posted jobs from settings");
                Thread.CurrentThread.Abort();
            }
        }

        public static IdaJob GetIdaJob(string model, string rec, string folder)
        {
            var list = IdaJobPool.Where(ida => ida.Model == model && ida.SubFolder == folder && ida.Record == rec).ToList();
            return list.Count == 0 ? null : list[0];
        }

        public static void LoadProject(string fileName)
        {
            // initiate TaskManager by passing the fileName
            List<string> allRuns = null;
            try
            {
                allRuns = File.ReadAllLines(fileName).ToList();
            }
            catch (Exception exception)
            {
                Logger.Error(exception.ToString() + "\nfailed to open " + fileName);
                Thread.CurrentThread.Abort();
            }
            if (allRuns.Count == 0)
            {
                Logger.Error(string.Format("the {0} file is empty; exiting", fileName));
                Thread.CurrentThread.Abort();
            }
            var ind = 0;
            while (ind < allRuns.Count)
            {
                var line = allRuns[ind];
                if (line.Length == 0 || line.StartsWith("#"))
                    allRuns.RemoveAt(ind);
                else
                {
                    ind++;
                }
            }
            SetIdaJobs(allRuns);
        }

        internal static void SetIdaJobs(List<string> lineList)
        {
            foreach (var line in lineList)
            {
                var list = line.Split(' ').ToList();
                //format:
                //ModelFolder Model <-skip skipList> <-records recList>
                var folder = list[0];
                var model = list[1];
                List<string> recList = Program.TheServer.AllRecordsList;
                if (list[2] == "-skip" || list[2] == "-Skip")
                {
                    var skipList = new List<string>();
                    for (var i = 3; i < list.Count; i++)
                    {
                        var word = list[i];
                        if (word.IndexOf("-", StringComparison.Ordinal) != -1)
                        {
                            break;
                        }
                        skipList.Add(word);
                    }
                    recList = new List<string>(Program.TheServer.NumAllRecs - skipList.Count);
                    var j = 0;
                    for (var i = 1; i <= Program.TheServer.NumAllRecs; i++)
                    {
                        var rec = i.ToString();
                        if (skipList.IndexOf(rec) != -1)
                            continue;
                        recList[j++] = rec;
                    }
                }
                else if (list[2] == "-records" || list[2] == "-Records")
                {
                    recList = new List<string>();
                    for (var i = 3; i < list.Count; i++)
                    {
                        var word = list[i];
                        if (word.IndexOf("-", StringComparison.Ordinal) != -1)
                            break;
                        recList.Add(word);
                    }
                }
                foreach (var task in recList.Select(rec => new IdaJob(folder, model, rec, Program.TheServer.TheTracer)))
                {
                    LoadedJobsPool.Add(task);
                }
            }
        }

        public static void PopulateActiveIdas()
        {
            if (LoadedJobsPool.Count == 0)
                return;
            var num = ClientManager.GetRgstrdList().Count;
            double rat = 0;
            try
            {
                rat = Convert.ToInt16(Program.TheServer.IdaNumToVlientPercentBox.Text);
            }
            catch
            {
                Logger.Error("Error reading Num. posted jobs from settings");
                Thread.CurrentThread.Abort();
            }
            rat /= 100.0;
            MaxActiveIdas = (int)(rat * num) - IdaJobPool.Count;
            if (MaxActiveIdas <= 0)
                return;
            MaxActiveIdas = Math.Min(MaxActiveIdas, LoadedJobsPool.Count);
            IdaJobPool.AddRange(LoadedJobsPool.GetRange(0, MaxActiveIdas));
            LoadedJobsPool.RemoveRange(0, MaxActiveIdas);
        }

        public static void PopulatePool()
        {
            foreach (var idaJob in IdaJobPool)
            {
                idaJob.Start();
            }
            foreach (var idaJob in IdaJobPool)
            {
                JobPool.AddJobs(idaJob.GetNextJobs(MaxPostedIms));
            }
        }

        public static bool SetResultString(string result, out SingleRunJob job, out IdaPoint pnt)
        {
            // model record im edp endTime dvrgFlg
            job = null;
            pnt = null;
            var resList = result.Split(' ').ToList();
            if (resList.Count < 2)
                return false;
            var model = resList[0];
            var rec = resList[1];
            var im = Convert.ToDouble(resList[2]);
            job = JobPool.GetJob(model, rec, im);
            if (job.TheStatus != SingleRunJob.Status.Cancelled)
                job.TheStatus = SingleRunJob.Status.Recieved;
            else
                job.TheStatus = SingleRunJob.Status.CancelRecieved;

            pnt = new IdaPoint
            {
                Im = Convert.ToDouble(resList[2]),
                Edp = Convert.ToDouble(resList[3]),
                AnalysisEndTime = Convert.ToDouble(resList[4]),
                DivergFlag = Convert.ToInt16(resList[5]) != 0
            };
            IdaJob toDelete = null;
            var jobList = IdaJobPool.Where(idajob => idajob.Model == model && idajob.Record == rec).ToArray();
            if (!jobList.Any())
                throw new Exception("IDAJob not found");
            var idaJob = jobList.ElementAt(0);
            bool cancel;
            bool deleteMe;
            idaJob.SetResult(job, pnt, out cancel, out deleteMe);
            if (cancel)
            {
                JobPool.CancelJobs(idaJob);
            }
            if (deleteMe)
            {
                toDelete = idaJob;
                Logger.LogIdaFinished(idaJob);
            }
            if (toDelete != null)
            {
                IdaJobPool.Remove(toDelete);
                if (LoadedJobsPool.Count == 0) return (IdaJobPool.Count == 0);
                IdaJobPool.Add(LoadedJobsPool[0]);
                LoadedJobsPool.RemoveAt(0);
            }
            return (IdaJobPool.Count == 0);
        }
    }
}
