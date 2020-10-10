using System;

namespace IDAServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LogFile = "";
            //: writes only to Console

            Logger.Start();

            Server.NumAllRecs = 44;
            Server.WorkingDir =
                @"C:\Users\Alireza\Desktop\ServerSide";
            Server.ProjectFile = "Project.txt"; // is located in WorkingDir
            Server.TheTracor = new HuntFillTracor
            {
                MaxClpsRsltn = 0.05,
                TetaLimit = 0.1,
                SlopeRatioLimit = 0.2,
                Im0 = 0.05,
                Step0 = 0.1,
                StepIncr = 0.05,
                MaxIdaPnts = 20
            };
        
            Server.Initiate();
            Server.Run();
            Logger.Close();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
