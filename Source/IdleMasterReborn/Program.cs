using System;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace IdleMasterExtended
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (var mutex = new Mutex(true, "IdleMasterReborn_SingleInstance_Mutex", out createdNew))
            {
                if (!createdNew)
                {
                    return;
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                Application.ThreadException += (o, a) => Logger.Exception(a.Exception);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Application.Run(new frmMain());
            }
        }
    }
}
