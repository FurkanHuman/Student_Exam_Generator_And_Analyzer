using App;
using Microsoft.Extensions.DependencyInjection;

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

            Application.Run(serviceProvider.GetRequiredService<SES_Main>());
        }
    }
}