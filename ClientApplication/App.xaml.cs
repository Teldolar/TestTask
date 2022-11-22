using ClientApplication.UI;
using Microsoft.Extensions.Configuration;
using NLog;
using ServerApplication.Lib;
using System;
using System.Windows;
using System.Windows.Input;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace ClientApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IConfiguration _configSettings;

        public App()
        {
            _configSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        private static void RunApplication()
        {
            var app = new App();
            var window = new MainWindow();
            app.Run(window);
        }

        public static int UserId { get; set; }

        public static string ServiceAddress
        {
            get
            {
                return _configSettings["serviceAddress"];
            }
        }

        public static string ServiceName
        {
            get
            {
                return _configSettings["serviceName"];
            }
        }
    }
}
