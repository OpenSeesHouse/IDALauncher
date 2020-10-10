using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace IDAClient
{
    public class ProcessManager
    {
        int _coreCount ;
        public static int ThreadCheckWaitSec = 2;
        //readonly List<Thread> MyThreads = new List<Thread>();
        List<ClientCore> _myCores;
        //private readonly List<ThreadState> LastState = new List<ThreadState>();
        public int MaxCpuUsage { get; set; }
        public static int NWorkers { get; set; }
        public string ClientAddress { get; set; }

        public ProcessManager(string clientAddress, string execPath, string tclModelName, int maxCpuUasge)
        {
            MaxCpuUsage = maxCpuUasge;
            ClientAddress = clientAddress;
            _coreCount = Environment.ProcessorCount;
            _myCores = new List<ClientCore>(_coreCount);
            for (var i = 1; i <= _coreCount; i++)
            {
                _myCores.Add(new ClientCore(i, clientAddress, execPath, tclModelName));
            }
        }


        public float GetCpuCounter()
        {

            PerformanceCounter cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";

            // will always start at 0
            var value = cpuCounter.NextValue();
            Thread.Sleep(1000);
            value = cpuCounter.NextValue();
            //Thread.Sleep(4000);
            //value += cpuCounter.NextValue();
            return value;
            //Thread.Sleep(3000);
            //firstValue = cpuCounter.NextValue();
            //return 0.5*(secondValue+firstValue);

        }


        public void StartHandling()
        {
            var freeCpu = MaxCpuUsage - GetCpuCounter();
            var nWorkers = (int)(Environment.ProcessorCount * freeCpu/100);
            if (nWorkers != NWorkers)
            {
                NWorkers = nWorkers;
                Logger.LogNworkersChanged();
            }
            var jobExists = true;
            var isWorking = false;
            while (jobExists || isWorking)
            {
                isWorking = false;
                var iWorker = 1;
                for (var i = 0; i < _coreCount; i ++)
                {
                    
                    var core = _myCores[i];
                    if (core.MyThread.ThreadState == ThreadState.Unstarted || _myCores[i].MyThread.ThreadState == ThreadState.Stopped)
                    {

                        string msg = "";
                        if (core.HasResult)
                        {
                            msg = core.GetResult();
                            core.HasResult = false;
                            Logger.LogJobFinished(core);
                        }
                        if (iWorker <= nWorkers)
                        {
                            iWorker++;
                            msg = $"{nWorkers}Request{msg}";
                            ClientManager.SendMessage(msg);
                            var answer = ClientManager.RecieveMessage(0, msg);
                            if (answer == "Done")
                            {
                                jobExists = false;
                                Logger.Log("There is no new job on server");
                            }
                            else
                            {
                                var job = MakeJob(answer);
                                core.Start(job);
                                Logger.LogNewJobStarted(core);
                                isWorking = true;
                            }
                        }
                        else
                            ClientManager.SendMessage(msg);
                    }
                    else
                    {
                        iWorker ++;
                        isWorking = true;
                    }
                }
                Thread.Sleep(ThreadCheckWaitSec*1000);
            }
            //ClientManager.Server.Close();
        }


        public Job MakeJob(string order)
        {
            var list = order.Split(' ').ToList();
            if (list.Count < 3)
            {
                Logger.Log("Bad job string: " + order);
                throw new Exception();
            }
            int rec = Convert.ToInt16(list[1]);
            var im = Convert.ToDouble(list[2]);
            return new Job(list[0], rec, im);
        }

    }
}
