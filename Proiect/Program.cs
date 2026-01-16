using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;

namespace Proiect;

static class Program
{
   
    static void Main()
    {
       
        ApplicationConfiguration.Initialize();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<ILogger, FileLogger>();
                services.AddSingleton<Clinica>();
                services.AddSingleton<AuthService>();
                services.AddSingleton<MainForm>();
                services.AddTransient<AdminForm>();
                services.AddTransient<MedicForm>();
                services.AddTransient<PacientForm>();
                services.AddTransient<SignupForm>();
            }).Build();

        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var mainForm = services.GetRequiredService<MainForm>();
            Application.Run(mainForm);
        }
    }
}