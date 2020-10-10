namespace IDAServer
{
    public class IdaPoint
    {
        public double Im { set; get; }
        public double Edp { set; get; }
        public double Slope { set; get; }
        public double RelSlope { set; get; }
        public double AnalysisEndTime { set; get; }
        public bool DivergFlag { set; get; }
        public bool CollapseFlag { set; get; }
        public string StringOut { get; set; }
        public int Order { get; set; }

        public IdaPoint()
        {
            CollapseFlag = false;
        }

        public IdaPoint(double im, double edp, double slop, double relslope, double endT, bool dvrgFlg, bool clpsFlg)
        {
            Im = im;
            Edp = edp;
            Slope = slop;
            RelSlope = relslope;
            DivergFlag = dvrgFlg;
            AnalysisEndTime = endT;
            CollapseFlag = clpsFlg;
        }
    }
}
