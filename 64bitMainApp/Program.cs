using System;
using System.Windows.Forms;

namespace AltInjector
{
    static class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Logger.Info("{PID}|Application started", System.Diagnostics.Process.GetCurrentProcess().Id);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                using (new SingleGlobalInstance(0))
                {
                    //Only 1 of these runs at a time
                    Application.Run(new TrayIconApp());
                }
            } catch (TimeoutException ex)
            {
                // Surpress this exception (multiple instances running)
                Logger.Error(ex, "{PID}|An instance of this application is already running!", System.Diagnostics.Process.GetCurrentProcess().Id);
            }
            Logger.Info("{PID}|Shutting down", System.Diagnostics.Process.GetCurrentProcess().Id);
            NLog.LogManager.Shutdown();
        }
    }
}
