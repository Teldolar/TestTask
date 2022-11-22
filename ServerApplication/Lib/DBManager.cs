using Microsoft.EntityFrameworkCore;
using ServerApplication.DB;
using ServerApplication.DB.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ServerApplication.Lib
{
    public class DBManager
    {
        private readonly SemaphoreSlim _dbContextSemaphore;
        private readonly ServerApplicationDBContext _dbContext;
        private readonly SenderManager _senderManager;

        public DBManager()
        {
            _dbContext = new ServerApplicationDBContext(Program.DefaultConnectionString);
            Task.Run(async () =>
            {
                await LogManager.AddLogAsync("Connected to DB", DateTime.Now);
            });
            _senderManager = new SenderManager();
            _dbContextSemaphore = new SemaphoreSlim(1, 1);
        }

        public void AddMouseLogAsync(MouseLogs mouseLogs, bool sendMail)
        {
            _dbContext.MouseLogs.Add(mouseLogs);
            _dbContext.SaveChanges();

            int count = _dbContext.MouseLogs.Count(p => p.UserId.Equals(mouseLogs.UserId));

            if ((count % 50).Equals(0) && sendMail)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("testarturgareev@mail.ru"),
                    Subject = "subject",
                    Body = $"<h1>На вашем аккаунте {count} записей</h1>",
                    IsBodyHtml = true,
                };

                _senderManager.SendMail(mailMessage, GetUserMail(mouseLogs.UserId));
                _senderManager.SendWhatsAppMessage($"На вашем аккаунте {count} записей", GetUserMobile(mouseLogs.UserId));
            }
        }

        public async Task<int> GetUserIdAsync(string login, string password)
        {
            await _dbContextSemaphore.WaitAsync();
            try
            {
                Users user = await _dbContext.Users.FirstOrDefaultAsync(p => p.Login.Equals(login) && p.Password.Equals(password));
                return user.Id;
            }
            finally
            {
                _dbContextSemaphore.Release();
            }
        }

        public List<MouseLogs> GetMouseLogs(int userId)
        {
            List<MouseLogs> mouseLogs = _dbContext.MouseLogs.Where(p => p.UserId.Equals(userId)).ToList();
            return mouseLogs;
        }

        public List<MouseLogs> GetMouseLogsByFilter(string filter)
        {
            List<MouseLogs> mouseLogs = _dbContext.MouseLogs.Where(p => p.Message.Contains(filter)).ToList();
            return mouseLogs;
        }

        public List<MouseLogs> GetMouseLogsByTime(DateTime startDate, DateTime endDate)
        {
            List<MouseLogs> mouseLogs = _dbContext.MouseLogs.Where(p => p.DateTime.Date >= startDate && p.DateTime.Date <= endDate).ToList();
            return mouseLogs;
        }

        public string GetUserMail(int userId)
        {
            Users user = _dbContext.Users.FirstOrDefault(p => p.Id.Equals(userId));
            return user.Mail;
        }

        public string GetUserMobile(int userId)
        {
            Users user = _dbContext.Users.FirstOrDefault(p => p.Id.Equals(userId));
            return user.Mobile;
        }

        public int GetMouseLogCount(int userId)
        {
            int mouseLogs = _dbContext.MouseLogs.Count(p => p.UserId.Equals(userId));
            return mouseLogs;
        }

        public int LogIn(string login, string password)
        {
            Users user = _dbContext.Users.FirstOrDefault(p => p.Login.Equals(login) && p.Password.Equals(password));
            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return -1;
            }
        }
    }
}