using System;
using System.Windows.Forms;

namespace VersionOneTFSServerConfig
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            foreach (string arg in args)
            {
                if (arg == "/unsubscribe")
                {
                    try
                    {
                        ConfigForm.TFSUnsubscribe();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error unsubscribing from TFS events, you should manually unsubscribe");
                    }
                    return;
                }

                if (arg == "/install")
                {
                    try
                    {
                        ConfigForm.Install();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Error setting .net version");
                    }
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigForm());
        }
    }
}