using ServerApplication.DB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServerApplication.Interfaces
{
    [ServiceContract]
    public interface IConnectionManager
    {
        [OperationContract]
        void LogMouseEvent(MouseLogs mouseLogs, bool sendMail);
        [OperationContract]
        void LogUserEvent(int userId, string message, DateTime dateTime);
        [OperationContract]
        List<MouseLogs> GetMouseLogs(int userId);
        [OperationContract]
        int LogIn(string login, string password);
        [OperationContract]
        int GetMouseLogCount(int userId);
        [OperationContract]
        List<MouseLogs> GetMouseLogsByFilter(string filter);
        [OperationContract]
        List<MouseLogs> GetMouseLogsByTime(DateTime startDate, DateTime endDate);
    }
}
