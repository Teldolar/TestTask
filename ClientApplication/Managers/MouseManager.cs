using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using ServerApplication.Lib;
using ServerApplication.Interfaces;
using ServerApplication.DB.Models;
using Autofac.Core;

namespace ClientApplication.Managers
{
    public class MouseManager
    {
        private double _mousePositionX;
        private double _mousePositionY;

        private ConnectionManager _connectionManager;

        public MouseManager(Point mousePosition, ConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;

            _mousePositionX = mousePosition.X;
            _mousePositionY = mousePosition.Y;
        }

        public MouseLogs MouseMovementProcessing(Point currentMousePosition, bool sendMail)
        {
            double deltaX = Math.Abs(currentMousePosition.X - _mousePositionX);
            double deltaY = Math.Abs(currentMousePosition.Y - _mousePositionY);
            if (deltaX >= 10)
            {
                MouseLogs mouseLogs = new MouseLogs()
                {
                    UserId = App.UserId,
                    Message = $"Mouse was moved to {deltaX} points horizontal",
                    DateTime= DateTime.Now,
                    PositionX= currentMousePosition.X,
                    PositionY= currentMousePosition.Y,
                };
                Task.Run(() =>
                {
                    _connectionManager.LogMouseEvent(mouseLogs, sendMail);
                });
                _mousePositionX = currentMousePosition.X;
                return mouseLogs;
            }
            if (deltaY >= 10)
            {
                MouseLogs mouseLogs = new MouseLogs()
                {
                    UserId = App.UserId,
                    Message = $"Mouse was moved to {deltaY} points vertical",
                    DateTime = DateTime.Now.Date,
                    PositionX = currentMousePosition.X,
                    PositionY = currentMousePosition.Y,
                };
                Task.Run(() =>
                {
                    _connectionManager.LogMouseEvent(mouseLogs, sendMail);
                });
                _mousePositionY = currentMousePosition.Y;
                return mouseLogs;
            }
            return null;
        }

        public MouseLogs MouseDownProcessng(MouseButton mouseButton, Point currentMousePosition, bool sendMail)
        {
            MouseLogs mouseLogs = new MouseLogs()
            {
                UserId = App.UserId,
                Message = $"The {mouseButton} was Down",
                DateTime = DateTime.Now,
                PositionX = currentMousePosition.X,
                PositionY = currentMousePosition.Y,
            };
            Task.Run(() =>
            {
                _connectionManager.LogMouseEvent(mouseLogs, sendMail);
            });
            return mouseLogs;
        }
    }
}
