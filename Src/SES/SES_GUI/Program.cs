using Application;
using Microsoft.Extensions.DependencyInjection;
using SES_GUI.UI;

namespace SES_GUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.


            ApplicationConfiguration.Initialize();
            using ServiceProvider serviceProvider = new ServiceCollection()
                .AddAppicationServiceRegistration()
                .AddSesGUIServiceRegistration()
                .BuildServiceProvider();

            string ConfPath = Paths.GetConfPath();
            string ConfFile = Paths.GetConfFile();

            if (!Directory.Exists(ConfPath))
                Directory.CreateDirectory(ConfPath);
            if (!File.Exists(ConfFile))
                System.Windows.Forms.Application.Run(serviceProvider.GetRequiredService<DbConnectionBuilder>());
            System.Windows.Forms.Application.Run(serviceProvider.GetRequiredService<SES_Main>());
        }
    }
}