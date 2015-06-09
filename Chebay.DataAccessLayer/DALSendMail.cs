using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Net.Mime;
using Shared.Entities;
using Shared.DataTypes;

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

        public void sendEmailNotification(string emailcomprador, DataProductoQueue p)
        {
            MailAddress toAddress;
            string subject;
            string body;

            if (emailcomprador == null)
            {
                toAddress = new MailAddress(p.OwnerProducto, "");
                subject = "Producto "+p.nombre;
                body = "<p>Lo sentimos... El producto " + p.ProductoID + " " + p.nombre + " ha alcanzado su fecha de finalizacion y no se ha vendido.</p>";
                //mando mail al que publico el producto informando que no se vendio
            }
            else
            {
                //aviso al usuario que gano la subasta
                toAddress = new MailAddress(emailcomprador, "");
                subject = "¡Has ganado una subasta en Chebay!";
                body = "<p>Has ganado una subasta sobre el producto " + p.ProductoID + " " + p.nombre + "!</p>";
            }
            
            
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
