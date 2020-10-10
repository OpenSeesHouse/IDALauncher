using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace IDAServer
{
    public abstract class ParallelTracer
    {
        public Dictionary<int, IdaPoint> TheGraph { set; get; }
        public bool StageChanged { set; get; }
        public bool Continue { set; get; }
        protected string OutFile = string.Empty;

        public abstract List<double> GetNextImsToPost(int num);

        public abstract List<int> GetNextPriosToPost(int num, int numWorkers);

        protected abstract void ReadInFile(string inFile);

        //update StageChanged and other stage variables
        public virtual void StartTracing(string outFile)
        {
            OutFile = outFile;
            TheGraph = new Dictionary<int, IdaPoint>();
            TheGraph[0] = new IdaPoint(0, 0, 0, 0, 0, false, false);
            if (File.Exists(outFile))
            {
                // the file exists and IDA must be continued
                // the arrays are filled and the status variables are updated
                // according to previous recording
                ReadInFile(outFile);
                try
                {
                    File.Copy(outFile, outFile + "_old", true);
                    File.Delete(outFile);
                }
                catch (Exception e)
                {
                    Logger.Error(e.ToString() + "\nFailed to open " + outFile + " for writing; exiting");
                    Thread.CurrentThread.Abort();
                }
            }
            
        }

        public abstract void UpdateStatus(ref IdaPoint pnt, SingleRunJob.Status theStatus);

        private void InsertPointInGraph(IdaPoint pnt)
        {
            var nn = pnt.Order;
            var maxN = 0;
            if (TheGraph.Count != 0)
                maxN = TheGraph.Keys.Max();
            for (var n = maxN; n >= nn; n--)
                TheGraph[n + 1] = TheGraph[n];
            TheGraph[nn] = pnt;
        }

        protected virtual void EndStatusUpdate(IdaPoint pnt)
        {
            // to be called by UpdateStatus() after each round of updating
            InsertPointInGraph(pnt);
            var line = FormatOutput(pnt) + Environment.NewLine;
            File.AppendAllText(OutFile, line);
        }

        protected virtual string FormatOutput(IdaPoint pnt)
        {
            var line = string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8}", pnt.Im, pnt.Edp, pnt.CollapseFlag,
                pnt.Slope, pnt.RelSlope, pnt.DivergFlag, pnt.AnalysisEndTime, pnt.StringOut, pnt.Order);
            return line;
        }

        public void IssueCollapse(IdaPoint pnt)
        {
            if (pnt == null)
                pnt = TheGraph[0];
            var newFilename = OutFile.Replace("IDA.txt", "Collapse.txt");
            var line = FormatOutput(pnt);
            File.WriteAllText(newFilename, line);
        }

        public IdaPoint GetPointAtIm(double im)
        {
            var resList = TheGraph.Values.Where(pnt => Math.Abs(pnt.Im - im) < 1e-2).ToList();
            if (resList.Count == 0)
                return null;
            return resList[0];
        }

        public virtual void EndTracing()
        {
            PrintGraph();
        }

        /*private void PrintImsAtEdps(double step)
        {
            var newFilename = OutFile.Replace("IDA.txt", "EdpIm.txt");
            var writer = File.CreateText(newFilename);
            var edp = 0.0;
            var edpList = TheGraph.Values.Select(pnt => pnt.Edp).ToList();
            var maxEdp = edpList.Max();
            while (edp < maxEdp)
            {
                edp += step;
                int num = edpList.TakeWhile(Edp => Edp < edp).Count();
                if (num >= TheGraph.Count)
                    break;
                num--;
                var y1 = TheGraph[num].Im;
                var y2 = TheGraph[num + 1].Im;
                var x1 = TheGraph[num].Edp;
                var x2 = TheGraph[num + 1].Edp;
                var im = y1 + (y2 - y1)/(x2 - x1)*(edp - x1);
                writer.WriteLine("{0} {1}", edp, im);
            }
            writer.Close();
        }*/

        private void PrintGraph()
        {
            var newFilename = OutFile.Replace(".txt", "_Sorted.txt");
            var writer = File.CreateText(newFilename);
            foreach (var line in TheGraph.Values.Select(FormatOutput))
            {
                writer.WriteLine(line);
            }
            writer.Close();
        }

        public abstract ParallelTracer GetCopy();
    }
}
