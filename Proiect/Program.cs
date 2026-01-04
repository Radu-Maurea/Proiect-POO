using System;
using System.Windows.Forms;

namespace Proiect
{
    static class Program
    {
        static void Main()
        {
            Clinica clinica = new Clinica();
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