using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.Lib
{
    public static class LogManager
    {
        public static async Task AddLogAsync(string message, DateTime dateTime)
        {
            await Task.Run(() =>
            {
                Logger logger = NLog.LogManager.GetCurrentClassLogger();
                logger.Info($"Message:{message} | DateTime:{dateTime}");
            });
        }
    }
}
