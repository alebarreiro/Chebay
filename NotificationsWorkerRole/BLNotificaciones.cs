using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Mail;
using System.Diagnostics;
using System.Net.Mime;
using Shared.Entities;
using Shared.DataTypes;

namespace NotificationsWorkerRole
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

        public void sendEmailNotification(Compra c, DataProductoQueue p)
        {
            MailAddress toAddress;
            string subject;
            string body;

            if (c == null)
            {
                toAddress = new MailAddress(p.OwnerProducto, "");
                subject = "Producto "+p.nombre;
                body = "<p>Lo sentimos... El producto " + c.ProductoID + " " + p.nombre + " ha alcanzado su fecha de finalizacion y no ha sido vendido.</p>";

                //mando mail al que publico el producto informando que no se vendio
                return;
            }
            //aviso al usuario que gano la subasta
            toAddress = new MailAddress(c.UsuarioID, "");
            subject = "¡Has ganado una subasta en Chebay!";
            body = "<p>Has ganado una subasta sobre el producto " + c.ProductoID + " " + p.nombre + "!</p>";

            
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
