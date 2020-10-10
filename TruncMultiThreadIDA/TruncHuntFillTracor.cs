using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDALauncher
{
    class TruncHuntFillTracor : ParallelIDATracor
    {
        double SaStart;
        double SaStep;
        double SaStepIncr;
        List<int> RecordList;
        int numRecs;
        double sa;
        Dictionary<int, RecordTraceData> recDataDict;
        double sa50_ = 0;
        double sa50 = 0;
        Stage stage = Stage.Hunt;
        int nPnt = 0;
        public override bool TraceFinished { set; get; }
        List<double> saVec = new List<double>();
        List<IDATaskData> TaskPool = new List<IDATaskData>();
        double clpsP = 0;
        double nonClpsP = 0;
        public List<string> OveralOutput { get; set; }

        public TruncHuntFillTracor(double saStart, double saStep, double saStepIncr, List<int> recList)
        {

            SaStart = saStart;
            SaStep = saStep;
            SaStepIncr = saStepIncr;
            RecordList = recList;
            numRecs = RecordList.Count;
            recDataDict = new Dictionary<int, RecordTraceData>(numRecs);
            foreach (var iRec in RecordList)
                recDataDict[iRec] = new RecordTraceData(iRec);
            OveralOutput = new List<string>();
            TraceFinished = false;
            sa = 0;
            saVec.Add(0);
            NextStep();
        }

        private void PopulateTaskPool()
        {
            TaskPool.Clear();
            foreach (var iRec in RecordList)
            {
                /*var data = recDataDict[iRec];
                if (data.SaClps != 0 && data.SaClps < sa)
                    continue;
                if (data.SaNoClps != 0 && data.SaNoClps > sa)
                    continue;
                var fnd = data.saVec.Find(s => Math.Abs(s - saVec[nPnt - 1]) < 1e-2);
                if (nPnt > 1 && fnd == 0)
                    TaskPool.Insert(0,new IDATaskData() { NumPnt = nPnt - 1, IM = saVec[nPnt - 1], Record = iRec, Priority = 0 });*/
                TaskPool.Add(new IDATaskData() {NumPnt = nPnt, IM = sa, Record = iRec, Priority = 0 });
            }

        }
        public override IDATaskData GetNewTask()
        {
            
            if (TaskPool.Count < 1)
                return null;
            var task = TaskPool[0];
            TaskPool.RemoveAt(0);
            return task;
        }

        private void NextStep()
        {
            var line = $"{nPnt} {sa} {stage} {clpsP}";
            foreach (var data in recDataDict.Values)
            {
                var flag = "?";
                if (data.SaClps != 0 && data.SaClps <= sa)
                {
                    flag = "1";
                }
                if (data.SaNoClps >= sa)
                {
                    flag = "0";
                }

                line = $"{line} {flag}";
            }
            OveralOutput.Add(line);
            nPnt++;
            if (clpsP >= 0.5)
            {
                stage = Stage.Bracket;
                sa50 = sa;
            }
            else
                sa50_ = sa;
            if (stage == Stage.Hunt)
                sa = SaStart + SaStep * (nPnt - 1) + SaStepIncr * (nPnt - 1) * nPnt * 0.5;
            else
            {
                var clpsRsltn = (sa50 - sa50_) / sa50_;
                if (clpsRsltn < 0.05)
                {
                    FinishTracing();
                    return;
                }
                sa = sa50_ + (sa50 - sa50_) / 3;
            }
            saVec.Add(sa);
            PopulateTaskPool();
        }

        private void FinishTracing()
        {
            TraceFinished = true;
            TaskPool.Clear();
            OveralOutput.Add($"MedIM= {sa50_}");
            foreach (var data in recDataDict.Values)
            {
                data.FinalizePrint();
            }
        }

        public override Object SetNewResult(IDATaskData task, IDATaskResult result)
        {

            var iRec = task.Record;
            var data = recDataDict[iRec];
            data.saVec.Add(task.IM);
            data.n++;
            data.failureFlag = result.FailFlag;
            data.endTime = result.EndTime;
            data.dispVec.Add(result.Disp);
            data.slope = Math.Abs((data.saVec[data.n] - data.saVec[data.n1]) / (data.dispVec[data.n] - data.dispVec[data.n1]));
            if (data.n == 1)
            {
                data.initSlope = data.slope;
            }
            data.slopeRat = data.slope / data.initSlope;
            if (data.slopeRat < 0.2 || data.dispVec[data.n] > 0.1 || data.failureFlag == 1)
            {
                data.clpsFlag = 1;
                data.SaClps = task.IM;

            }
            else
            {
                data.clpsFlag = 0;
                data.SaNoClps = task.IM;
                data.n1 = data.n;

            }
            data.PrintSelf();

            var numClpsd = 0.0;
            /*var numNonClps = 0.0;
            foreach (var iData in recDataDict.Values)
            {
                if (iData.SaClps != 0 && iData.SaClps <= sa)
                {
                    numClpsd++;
                }
                if (iData.SaNoClps >= sa)
                {
                    numNonClps++;
                }
            }
            clpsP = numClpsd / numRecs;
            nonClpsP = numNonClps / numRecs;
            if (nPnt > 1 && (clpsP >= 0.5 || nonClpsP >= 0.5))
                NextStep();
            else if (nonClpsP >= (numRecs - 1 + 0.0) / (numRecs))
                NextStep();*/
            var numRcvd = 0;
            foreach (var iData in recDataDict.Values)
            {
                if (iData.n == nPnt)
                    numRcvd++;
                if (iData.SaClps != 0 && iData.SaClps <= sa)
                    numClpsd++;
            }
            if (numRcvd == numRecs)
            {
                clpsP = numClpsd / numRecs;
                NextStep();
            }
            return data;
        }
    }

    internal class StepData
    {
        double clpsP;
        int n;
        double sa;
        Stage stage;
    }
}
