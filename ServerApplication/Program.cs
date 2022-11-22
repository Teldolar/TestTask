using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Config;
using NLog.Targets;
using ServerApplication.DB;
using ServerApplication.Interfaces;
using ServerApplication.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace ServerApplication
{
    internal class Program
    {
        private static IConfiguration _configSettings;


        public static string DefaultConnectionString
        {
            get
            {
                return _configSettings["ConnectionStrings:DefaultConnection"];
            }
        }

        private static string s_serviceAddress
        {
            get
            {
                return _configSettings["serviceAddress"];
            }
        }

        private static string s_serviceName
        {
            get
            {
                return _configSettings["serviceName"];
            }
        }

        static void Main(string[] args)
        {
            BuildConfigSettings();

            connectToServer();

            Console.ReadKey();
        }

        private static void connectToServer()
        {
            var host = new ServiceHost(typeof(ConnectionManager), new Uri($"http://{s_serviceAddress}/{s_serviceName}"));
            var serverBinding = new NetHttpBinding();
            serverBinding.MaxReceivedMessageSize= 2147483647;
            host.AddServiceEndpoint(typeof(IConnectionManager), serverBinding, "");
            host.Open();
        }

        private static void BuildConfigSettings()
        {
            _configSettings = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
