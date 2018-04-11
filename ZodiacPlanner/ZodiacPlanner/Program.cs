using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZodiacPlanner
{
    static class Program
    {

        public static ColorManager colors;
        public static Queue<string> log;
        public static Settings settings;

        const string LOGFILE = "log.txt";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            colors = new ColorManager();
            log = new Queue<string>(300);

            settings = Settings.Load();

            Application.Run(new Form1());
            colors.Save();
            settings.Save();
        }

        public static void Log(string message, Label label)
        {
            if (log.Count() == 300)
                log.Dequeue();
            log.Enqueue(message);

            if(label != null)
                label.Text = message;

            if (settings.saveLog)
                File.AppendAllText(LOGFILE, $"{message}\n");
        }
    }
}
