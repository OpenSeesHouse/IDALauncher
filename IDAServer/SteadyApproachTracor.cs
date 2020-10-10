using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IDAServer
{
    public class SteadyApproachTracor:ParallelTracor
    {
        public int NumPostedIms; 
        public enum Stage
        {
            Hunt = 0, Approach = 1, Fill = 2
        }

        // Setting Variables:
        public double MaxClpsRsltn { set; get; }
        public double TetaLimit { set; get; }
        public double SlopeRatioLimit { set; get; }
        public double Im0 { set; get; }
        public double Step0 { set; get; }
        public double StepIncr { set; get; }
        public int MaxIdaPnts { set; get; }

        // Status Variables:
        public double InitSlope { get; set; }
        public double ImStable { set; get; }
        public double EdpStable { set; get; }
        public double ImCollapse { set; get; }
        public double ImForNexts { set; get; }
        public int NLastStable { set; get; }
        public Stage CurStage { set; get; }

        public override List<double> GetNextImsToPost(int num)
        {
            // check sa == 0 for end of IDA
            var toPostList = new List<double>(num);
            for (var i = 0; i < num; i++)
            {
                if (NumPostedIms != 0 && Math.Abs(ImForNexts) < 1e-3)
                {
                    var res = new List<double>(1) {0};
                    return res;
                }
                toPostList.Add(ImForNexts);

                if (CurStage == Stage.Hunt)
                {
                    ImForNexts += Step0 + (NumPostedIms - 1)*StepIncr;
                }
                else if (CurStage == Stage.Bracket)
                {
                    ImForNexts += (ImCollapse - ImForNexts) / 3.0;
                }
                else
                {
                    if (NLastStable >= 1)
                    {
                        ImForNexts = (TheGraph[NLastStable].Im + TheGraph[NLastStable - 1].Im) / 2.0;
                        NLastStable--;
                    }
                    else
                    {
                        ImForNexts = 0;
                    }
                }
                if (Math.Abs(ImForNexts) < 1e-3)
                    break;
                NumPostedIms++;
            }
            return toPostList;
        }

        public override List<int> GetNextPriosToPost(int num, int nWorkers)
        {
            int fStage;
            int gFac;
            switch (CurStage)
            {
                case Stage.Hunt:
                    fStage = 10000;
                    gFac = 1;
                    break;
                case Stage.Bracket:
                    fStage = 1000;
                    gFac = 0;
                    break;
                default:
                    fStage = 0;
                    gFac = 0;
                    break;
            }
            //if (Math.Abs(ImForNexts) < 1e-3)
            //    return new List<int>(0);
            var toPostList = new List<int>(num);
            var nn = NumPostedIms - num + 1;
            for (var i = 0; i < num; i++)
            {
                var priority = fStage + gFac * (nn + i) + 5 * nWorkers;
                toPostList.Add(priority);
            }
            return toPostList;
        }

        public override void StartTracing(string outFile)
        {
            TheGraph = new Dictionary<int, IdaPoint>(MaxIdaPnts);
            NumPostedIms = 0;
            InitSlope = 0;
            ImStable = 0;
            EdpStable = 0;
            ImCollapse = 1e32;
            ImForNexts = Im0;
            NLastStable = 1;
            CurStage = Stage.Hunt;
            Continue = true;
            base.StartTracing(outFile);
        }

        protected override void ReadInFile(string inFile)
        {
            List<string> allLines = new List<string>();
            try
            {
                allLines = File.ReadAllLines(inFile).ToList();
            }
            catch (Exception e)
            {
                Logger.Log("Hunt-Fill Tracor:");
                Logger.Log(e.ToString());
                Logger.Log("Cannot read data from existing " + inFile);
            }

            // outputLine format:
            // sa teta clpsFlg slope slopRat dvrgFlg stage endTime nn
            int nn = 1;
            foreach (var line in allLines)
            {
                var lineList = line.Split(' ').ToList();
                IdaPoint pnt = new IdaPoint();
                var im = Convert.ToDouble(lineList[0]);
                pnt.Im = im;
                pnt.Edp = Convert.ToDouble(lineList[1]);
                pnt.CollapseFlag = Convert.ToBoolean(lineList[2]);
                pnt.Slope = Convert.ToDouble(lineList[3]);
                pnt.RelSlope = Convert.ToDouble(lineList[4]);
                pnt.DivergFlag = Convert.ToBoolean(lineList[5]);
                pnt.AnalysisEndTime = Convert.ToDouble(lineList[6]);
                TheGraph.Add(nn, pnt);
                if (pnt.CollapseFlag)
                {
                    ImCollapse = pnt.Im;
                }
                else
                {
                    ImStable = pnt.Im;
                    EdpStable = pnt.Edp;
                    if (CurStage == Stage.Hunt)
                    {
                        NLastStable = TheGraph.Count; //to support for file continuation
                    }
                }
                CurStage = lineList[6] == "0" ? Stage.Hunt : (lineList[6] == "1" ? Stage.Bracket : Stage.Fill);
                if (nn >= MaxIdaPnts)
                {
                    Continue = false;
                }
                nn++;
            }
            
        }

        public override void UpdateStatus(ref IdaPoint pnt, SingleRunJob.Status theStatus)
        {
            //iterate through all points to find the order
            var im = pnt.Im;
            var edp = pnt.Edp;
            var dvrgFlg = pnt.DivergFlag;
            var nn = TheGraph.TakeWhile(kvp => kvp.Value.Im < im).Count();
            StageChanged = false;
            var stage0 = CurStage;
            if (CurStage != Stage.Fill)
            {
                pnt.Slope = (pnt.Im - ImStable) / (edp - EdpStable);
                pnt.Slope = Math.Abs(pnt.Slope);
                if (nn == 1 && Math.Abs(InitSlope) < 1e-3)
                    InitSlope = pnt.Slope;
                pnt.RelSlope = pnt.Slope / InitSlope;
                if (edp > TetaLimit || pnt.RelSlope < 0.2 || dvrgFlg)
                {
                    pnt.CollapseFlag = true;
                    CurStage = Stage.Bracket;
                    if (pnt.Im < ImCollapse)
                    {
                        ImCollapse = pnt.Im;
                        ImForNexts = ImStable + (ImCollapse - ImStable) / 3;
                        StageChanged = true;
                    }
                }
                else
                {
                    if (pnt.Im > ImStable)
                    {
                        ImStable = pnt.Im;
                        EdpStable = edp;
                    }
                    if (CurStage == Stage.Hunt)
                    {
                        NLastStable = nn;
                    }
                }
                if (CurStage == Stage.Bracket)
                {
                    var clpseRsln = (ImCollapse - ImStable)/ImStable;
                    if (clpseRsln < MaxClpsRsltn)
                    {
                        CurStage = Stage.Fill;
                        StageChanged = true;
                        ImForNexts = (TheGraph[NLastStable].Im + TheGraph[NLastStable - 1].Im)/2.0;
                        NLastStable--;
                        if (NLastStable < 1)
                        {
                            Continue = false;
                        }
                    }
                }
            }
            pnt.StringOut = string.Format("{0} {1}", CurStage, theStatus);
            pnt.Order = nn;
            EndStatusUpdate(pnt);
        }

        public override ParallelTracor GetCopy()
        {
            return (HuntFillTracor) MemberwiseClone();
/*
            return new HuntFillTracor()
            {
                MaxClpsRsltn = MaxClpsRsltn,
                TetaLimit = TetaLimit,
                SlopeRatioLimit = SlopeRatioLimit,
                Im0 = Im0,
                Step0 = Step0,
                StepIncr = StepIncr,
                MaxIdaPnts = MaxIdaPnts
            };
*/
        }
    }
}
