﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.ServiceModel.Channels;

namespace ServerApplication.Lib
{
    public class SenderManager
    {
        private SmtpClient _smtpClient;
        private string _url; // your instanceId
        private string _token;         //instance Token
        private RestClient _restClient;

        public SenderManager()
        {
            _smtpClient = new SmtpClient("smtp.mail.ru")
            {
                Port = 587,
                Credentials = new NetworkCredential("testarturgareev@mail.ru", "BmC8UxDrwDLHVguQgLnj"),
                EnableSsl = true,
            };

            _token = "85sply5ryi5w74mb";

            _url = "https://api.ultramsg.com/instance23841/messages/chat";
            _restClient = new RestClient(_url);
        }

        public void SendMail(MailMessage message, string mail)
        {
            message.To.Add(mail);
            _smtpClient.Send(message);
        }

        public void SendWhatsAppMessage(string message, string mobile)
        {
            var request = new RestRequest(_url, Method.Post);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("token", _token);
            request.AddParameter("to", mobile);
            request.AddParameter("body", message);

            RestResponse response = _restClient.Execute(request);
        }
    }
}
