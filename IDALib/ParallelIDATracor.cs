using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDALauncher
{
    public class IDATaskData
    {
        public double IM { set; get; }
        public int Record { set; get; }
        public int Priority { set; get; }
        public int NumPnt { set; get; }
    }

    public class IDATaskResult
    {
        public int FailFlag;
        public double Disp;
        public double EndTime;
    }
    public abstract class ParallelIDATracor
    {
        public abstract bool TraceFinished { get; set; }
        public abstract Object SetNewResult(IDATaskData task, IDATaskResult result);
        public abstract IDATaskData GetNewTask();
    }
}
