using System;
using System.Windows.Forms;

namespace Proiect
{
    static class Program
    {
        static void Main()
        {
            ILogger logger = new FileLogger();
            Clinica clinica = new Clinica(logger);
            foreach (var user in clinica.UtilizatoriReadOnly)
            {
                Console.WriteLine(user);
            }
            
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
            
            
        }

    }
}