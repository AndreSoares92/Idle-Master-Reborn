using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace IdleMasterReborn
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

                MigrateLegacyUserConfig();

                Application.Run(new frmMain());
            }
        }

        private static void MigrateLegacyUserConfig()
        {
            try
            {
                string appDataDir = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "IdleMasterReborn");
                if (!Directory.Exists(appDataDir))
                    return;

                foreach (string configPath in Directory.GetFiles(appDataDir, "user.config", SearchOption.AllDirectories))
                {
                    string content = File.ReadAllText(configPath, Encoding.UTF8);
                    if (content.Contains("IdleMasterExtended.Properties.Settings"))
                    {
                        content = content.Replace("IdleMasterExtended.Properties.Settings", "IdleMasterReborn.Properties.Settings");
                        File.WriteAllText(configPath, content, new UTF8Encoding(false));
                    }
                }
            }
            catch
            {
            }
        }
    }
}
