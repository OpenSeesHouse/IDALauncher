
using System.Collections.Generic;
using System.Linq;

namespace IDAClient
{
    public class Result
    {
        public List<string> ResList ;
        private readonly Job _doneJob;
        public Result(Job myJob, List<string> res )
        {
            ResList = new List<string>();
            ResList = res;
            _doneJob = myJob;
        }

        public string GetMassage()
        {
            var massage = _doneJob.ModelFullPath + " " + _doneJob.Record + " " + _doneJob.Im + " ";
            massage = ResList.Aggregate(massage, (current, item) => current + (item + " "));
            return massage;
        }
    }
}
