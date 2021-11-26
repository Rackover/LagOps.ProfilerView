using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LouveSystems.LagOps.ProfilerView
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var window = new MainWindow();

            if (args.Length > 0)
            {
                window.Load(System.IO.File.ReadAllText(args[0]));
            }

            Application.Run(window);
        }
    }
}
