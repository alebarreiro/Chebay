﻿using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace DataAccessLayer
{
    public class BLNotificaciones
    {
        private SmtpClient smtp;
        private MailAddress fromAddress;
        private string fromPassword = "#!Chebay";

        public BLNotificaciones()
        {
            fromAddress = new MailAddress("chebaysend@gmail.com", "Chebay");

            smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
        }

        public void sendEmailNotification(string email, string asunto, string mensaje)
        {
            MailAddress toAddress = new MailAddress(email);

            try
            {
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = asunto,
                    IsBodyHtml = true,
                    Body = mensaje
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error enviar mensaje: "+ e.Message);
            }
        }
    }
}
