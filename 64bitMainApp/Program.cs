using System;
using System.Windows.Forms;

namespace AltInjector
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                using (new SingleGlobalInstance(0))
                {
                    //Only 1 of these runs at a time
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                }
            } catch (TimeoutException)
            {
                // Surpress this exception (multiple instances running)
            }
        }
    }
}
