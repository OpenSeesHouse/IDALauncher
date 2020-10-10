using System;
using System.Collections.Generic;
using System.IO;
using Utilities;

namespace IDALauncher
{
    public enum Stage
    {
        Hunt = 0, Bracket = 1, Fill = 2
    }
    public class RecordTraceData
    {
        public List<double> dispVec = new List<double>();
        public List<string> lineVec = new List<string>();
        public List<double> saVec = new List<double>();
        public double initSlope = 0.0;
        public double slopeRat = 0.0;
        public double slope = 0.0;
        public double SaClps = 0;
        public double SaNoClps = 0;
        public int n1 = 0;
        public int n = 0;
        public int clpsFlag = 0;
        public StreamWriter Out1, Out2;
        public int failureFlag;
        public double endTime;
        public string stage;

        public RecordTraceData(int iRec)
        {
            Directory.CreateDirectory($"TruncIDA/{iRec}");
            Out1 = File.CreateText($"TruncIDA/{iRec}/IDAUnsorted.txt");
            Out2 = File.CreateText($"TruncIDA/{iRec}/IDA.txt");
            dispVec.Add(0);
            saVec.Add(0);
            lineVec.Add("0 0");
        }

        public void PrintSelf()
        {
            var line = $"{saVec[n]} {dispVec[n]} {slope} {slopeRat} {clpsFlag} {failureFlag} {endTime} {stage}";
            lineVec.Add(line);
            Out1.WriteLine(line);
            Out1.Flush();
        }

        public void FinalizePrint()
        {
            Utils.SortArray(ref lineVec, ref saVec);

            for (var i = 1; i < lineVec.Count; i++)
            {
                Out2.WriteLine(lineVec[i]);
            }
            Out1.Close();
            Out2.Close();
        }
    }
}
