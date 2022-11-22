using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ServerApplication.DB.Models;
using ServerApplication.Interfaces;
using ServerApplication.Lib;

namespace ClientApplication.Managers
{
    public class ConnectionManager
    {
        private Uri _tcpUri;
        private EndpointAddress _address;
        private NetHttpBinding _clientBinding;
        private ChannelFactory<IConnectionManager> _factory;
        private IConnectionManager _service;

        public ConnectionManager()
        {
            Uri uri = new Uri($"http://{App.ServiceAddress}/{App.ServiceName}");
            _tcpUri = uri;
            _address = new EndpointAddress(_tcpUri);
            _clientBinding = new NetHttpBinding();
            _clientBinding.MaxReceivedMessageSize = 2147483647;
            _factory = new ChannelFactory<IConnectionManager>(_clientBinding, _address);
            _service = _factory.CreateChannel();
        }

        public async void LogMouseEvent(MouseLogs mouseLogs, bool sendMail)
        {
            await Task.Run(() =>
            {
                _service.LogMouseEvent(mouseLogs, sendMail);
            });
        }

        public async Task LogUserEvent(int userId, string message, DateTime dateTime)
        {
            await Task.Run(() =>
            {
                _service.LogUserEvent(userId, message, dateTime);
            });
        }

        public List<MouseLogs> GetMouseLogs(int userId)
        {
            return _service.GetMouseLogs(userId);
        }

        public List<MouseLogs> GetMouseLogsByFilter(string filter)
        {
            return _service.GetMouseLogsByFilter(filter);
        }

        public List<MouseLogs> GetMouseLogsByTime(DateTime startDate, DateTime endDate)
        {
            return _service.GetMouseLogsByTime(startDate, endDate);
        }

        public int LogIn(string login, string password)
        {
            return _service.LogIn(login, password);
        }

        public int GetMouseLogCount(int userId)
        {
            return Task.Run(() =>
            {
                return _service.GetMouseLogCount(userId);
            }).Result;
        }
    }
}
