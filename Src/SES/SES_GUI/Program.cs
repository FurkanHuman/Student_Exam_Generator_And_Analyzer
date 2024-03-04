using App;
using Microsoft.Extensions.DependencyInjection;
using SES_GUI.UI;
using System;
using System.IO;
using System.Windows.Forms;

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
                .AddAppServiceRegistration()
                .AddSesGUIServiceRegistration()
                .BuildServiceProvider();
 
            string ConfPath = Paths.GetConfPath();
            string ConfFile = Paths.GetConfFile();

            if (!Directory.Exists(ConfPath))
                Directory.CreateDirectory(ConfPath);
            if (!File.Exists(ConfFile))
                Application.Run(serviceProvider.GetRequiredService<DbConnectionBuilder>());

            Application.Run(serviceProvider.GetRequiredService<SES_Main>());
        }
    }
}