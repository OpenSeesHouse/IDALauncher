using System;
using System.Windows.Forms;

namespace IDAServer
{
    static class Program
    {
        public static Server TheServer;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            TheServer = new Server();
            Application.Run(TheServer);
        }
    }
}
