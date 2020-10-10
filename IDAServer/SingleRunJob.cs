namespace IDAServer
{
    public class SingleRunJob:Job
    {
        public double Im { set; get; }
        public int Priority { set; get; }
        public string ModelFile { set; get; }
        public IdaJob MyIdaJob { set; get; }

        public enum Status
        {
            Waiting = 0,
            Posted = 1,
            Recieved = 2,
            Cancelled = 3,
            CancelRecieved = 4
        };

        public Status TheStatus { set; get; }

        public SingleRunJob(string folder, string model, string rec, double im, int prio):base(folder, model, rec)
        {
            Im = im;
            TheStatus = Status.Waiting;
            Priority = prio;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Model, Record, Im);
        }

    }
}
