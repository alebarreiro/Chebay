using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Diagnostics;
using System.Net.Mime;
using Shared.Entities;

namespace Chebay.BusinessLogicLayer
{
    public class BLNotificaciones
    {
        private static string user = "azure_404bbab86c074fcb579f83fa985aa1c2@azure.com";
        private static string pass = "piEe0cK4Fn5uIIo";

        public void sendEmailNotification(Compra c, Producto p)
        {
            // Create the email object first, then add the properties.
            var myMessage = new SendGridMessage();

            // Add the message properties.
            myMessage.From = new MailAddress("no-reply@chebay.com");
            
            // Add multiple addresses to the To field.
            List<String> recipients = new List<String>
            {
                @"Alejandro Añón <" + c.UsuarioID + ">",
            };

            myMessage.AddTo(recipients);

            myMessage.Subject = "¡Has ganado una subasta en Chebay!";

            //Add the HTML and Text bodies
            myMessage.Html = "<p>Has ganado una subasta sobre el producto " + c.ProductoID + " " + p.nombre + "!</p>";
            //myMessage.Text = "Hello World plain text!";

            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(user, pass);
            
            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email.
            // You can also use the **DeliverAsync** method, which returns an awaitable task.
            transportWeb.DeliverAsync(myMessage);
        }

       

    }
}
