using ServerApplication.DB.Models;
using ServerApplication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerApplication.Lib
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly DBManager _dbManager;

        public ConnectionManager()
        {
            _dbManager = new DBManager();
        }

        public void LogMouseEvent(MouseLogs mouseLogs, bool sendMail)
        {
            _dbManager.AddMouseLogAsync(mouseLogs, sendMail);
        }

        public async void LogUserEvent(int userId, string message, DateTime dateTime)
        {
            await LogManager.AddLogAsync(message, dateTime);
        }

        public List<MouseLogs> GetMouseLogsByTime(DateTime startDate, DateTime endDate)
        {
            return _dbManager.GetMouseLogsByTime(startDate, endDate);
        }

        public List<MouseLogs> GetMouseLogs(int userId)
        {
            return _dbManager.GetMouseLogs(userId);
        }

        public int GetMouseLogCount(int userId)
        {
            return _dbManager.GetMouseLogCount(userId);
        }

        public int LogIn(string login, string password)
        {
            return _dbManager.LogIn(login, password);
        }

        public List<MouseLogs> GetMouseLogsByFilter(string filter)
        {
            return _dbManager.GetMouseLogsByFilter(filter);
        }
    }
}
