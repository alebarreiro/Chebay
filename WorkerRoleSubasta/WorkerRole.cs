using DataAccessLayer;
using Microsoft.Azure;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Shared.DataTypes;
using Shared.Entities;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace WorkerRoleSubasta
{
    public class WorkerRole : RoleEntryPoint
    {
        // Nombre de la cola
        const string QueueName = "subasta";

        // QueueClient es seguro para subprocesos. Se recomienda almacenarlo en caché 
        // en lugar de crearlo de nuevo con cada solicitud
        QueueClient Client;
        ManualResetEvent CompletedEvent = new ManualResetEvent(false);


        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        private void procesarSubasta(DataProductoQueue p)
        {
            IDALSubasta ip = new DALSubastaEF();
            BLNotificaciones bl = new BLNotificaciones();
            IDALUsuario ubl = new DALUsuarioEF();

            //obtengo producto
            Producto prod = ip.ObtenerProducto(p.ProductoID, p.TiendaID);

            //Si no fue comprado por CompraDirecta
            if (!ip.ExisteCompra(p.ProductoID, p.TiendaID))
            {
                //vendedor
                Usuario vendedor = ubl.ObtenerUsuario(p.OwnerProducto, p.TiendaID);

                if (ip.TieneOfertas(p.ProductoID, p.TiendaID))
                {
                    //Obtengo la oferta ganadora.
                    Oferta o = ip.ObtenerOfertaGanadora(p.ProductoID, p.TiendaID);

                    //Creo una compra entre el Producto y el Usuario.
                    Compra c = new Compra
                    {
                        fecha_compra = p.fecha_cierre,
                        monto = o.monto,
                        ProductoID = o.ProductoID,
                        UsuarioID = o.UsuarioID
                    };
                    //se envia mensaje al vendedor en AgregarCompra
                    ip.AgregarCompraPostSubasta(c, p.TiendaID);

                    //obtengo mail ganador           
                    Usuario u = ubl.ObtenerUsuario(c.UsuarioID, p.TiendaID);
                    //Notifico al ganador.
                    String linkTienda = "http://chebuynow.azurewebsites.net/" + p.TiendaID;
                    String linkCalif = "http://chebuynow.azurewebsites.net/" + p.TiendaID + "/Usuario/CalificarUsuario?prodId=" + p.ProductoID;
                    string bodyMensaje = getBodyMailHTML(p.TiendaID + ": Subasta ganada!", "Has ganado una nueva subasta!", u, prod, c.monto, true, linkCalif, linkTienda);
                       
                    string asunto = "Chebay: Subasta de producto!";
                    bl.sendEmailNotification(u.Email, asunto, bodyMensaje);


                    //en agregar compra se notifica al vendedor.

                    //mail a vendedor
                    //obtengo vendedor
                    IDALUsuario udal = new DALUsuarioEF();

                    string asuntovendedor = "Chebay: Venta de producto!";
                    string bodyMensajeVendedor = getBodyMailHTML(p.TiendaID + ": Producto vendido!", "Has vendido un nuevo producto!", vendedor, prod, c.monto, false, "", linkTienda);

                    if (vendedor.Email != null)
                    {
                        bl.sendEmailNotification(vendedor.Email, asuntovendedor, bodyMensajeVendedor);
                    }
                    else
                    {
                        if (IsValidMail(vendedor.UsuarioID))
                        {
                            bl.sendEmailNotification(vendedor.UsuarioID, asuntovendedor, bodyMensajeVendedor);
                        }
                        //else... por algun error no tiene mail
                    }
         
                }
                else
                {
                    //no fue comprado
                    Debug.WriteLine("NO TIENE COMPRAS MAIL AL VENDEDOR");
                    String linkTienda = "http://chebuynow.azurewebsites.net/" + p.TiendaID;
                    string asunto = "Chebay: Producto no vendido...";
                    string bodyMensajeVendedor = getBodyMailHTML(p.TiendaID + ": Producto no vendido...", "No se ha logrado subastar su producto.", vendedor, prod, 0, false, "", linkTienda);
                    bl.sendEmailNotification(vendedor.Email, asunto, bodyMensajeVendedor);
                }
                
            }//si fue comprado se descarta el mensaje.
         
        }


        public override void Run()
        {
            Trace.WriteLine("Iniciando el procesamiento de mensajes");

            // Inicia el bombeo de mensajes y se invoca una devolución de llamada para cada mensaje que se recibe. Si se llama a close en el cliente, se detendrá el bombeo.
            Client.OnMessage((receivedMessage) =>
                {
                    try
                    {
                        // Procesar el mensaje
                        DataProductoQueue message = receivedMessage.GetBody<DataProductoQueue>();
                        Trace.WriteLine("Procesando mensaje del producto " + message.nombre);
                        procesarSubasta(message);
                    }
                    catch
                    {
                        Trace.WriteLine("ERROR AL PROCESAR MENSAJE QUEUE SUBASTA...");
                        // Controlar cualquier excepción específica del procesamiento de mensajes aquí
                    }
                });

            CompletedEvent.WaitOne();
        }

        public override bool OnStart()
        {
            // Establecer el número máximo de conexiones concurrentes. 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Crear la cola si no existe aún
            string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
            if (!namespaceManager.QueueExists(QueueName))
            {
                namespaceManager.CreateQueue(QueueName);
            }

            // Inicializar la conexión con la cola de Service Bus
            Client = QueueClient.CreateFromConnectionString(connectionString, QueueName);
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Cerrar la conexión con la cola de Service Bus
            Client.Close();
            CompletedEvent.Set();
            base.OnStop();
        }

        /**
         * Función para mandar un mail formato HTML al cliente o al vendedor de producto
         * 
         * {titulo} Subasta ganada! ó Producto vendido!
         * {motivo} Has ganado una nueva subasta ó Has vendido un nuevo producto
         * {u} Usuario vendedor o comprador
         * {p} Producto vendido o comprado
         * {monto} Monto de la compra / venta
         * {calificacion} true si es para el usuario que compró el producto
         * {linkCalif} Si calificacion = true http://chebuynow.azurewebsites.net/{tiendaId}/Usuario/CalificarUsuario?prodId={prodId}
         *             Si calificacion = false ""
         * {linkWeb}  http://chebuynow.azurewebsites.net/tiendaId
         */
        public string getBodyMailHTML(String titulo, String motivo, Usuario u, Producto p, int monto, bool calificacion, String linkCalif, String linkWeb)
        {
            String dest = "";
            String fecha = DateTime.UtcNow.ToString();
            if (u.Nombre != "" || u.Apellido != "")
            {
                dest = u.Nombre + " " + u.Apellido;
            }
            else
            {
                dest = u.UsuarioID;
            }
            String cuerpo = "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n"
                    + "<head>\n"
                    + "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n"
                    + "<title>" + titulo + "</title>\n"
                    + "</head>\n"
                    + "\n"
                    + "<body>\n"
                    + "<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "  <tr>\n"
                    + "    <td align=\"center\" valign=\"top\" bgcolor=\"#ffe77b\" style=\"background-color:#ffe77b;\"><br>\n"
                    + "    <br>\n"
                    + "    <table width=\"600\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "      <tr>\n"
                    + "      </tr>\n"
                    + "      <tr>\n"
                    + "        <td align=\"left\" valign=\"top\" bgcolor=\"#f89406\" style=\"background-color:#f89406; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><div style=\"font-size:46px; color:#ff5500;\"><b>" + titulo + "</b></div>\n"
                    + "  <div style=\"font-size:13px; color:#000;\"><b>Estimado cliente " + dest + ", " + motivo + "</b></div>\n"
                    + "          </td>\n"
                    + "      </tr>\n"
                    + "      <tr>\n"
                    + "        <td align=\"left\" valign=\"top\" bgcolor=\"#ffffff\" style=\"background-color:#ffffff;\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "          <tr>\n"
                    + "            <td align=\"center\" valign=\"middle\" style=\"padding:10px; color:#ff5500; font-size:42px; font-family:Georgia, 'Times New Roman', Times, serif;\">" + p.nombre + "</td>\n"
                    + "          </tr>\n"
                    + "        </table>\n"
                    + "          <table width=\"95%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n"
                    + "            <tr>\n"
                    + "              <td align=\"left\" valign=\"middle\" style=\"color:#525252; font-family:Arial, Helvetica, sans-serif; padding:10px;\">\n"
                    + "              <div style=\"font-size:12px;\">" + p.descripcion + "</div>\n"
                    + "              </td>\n"
                    + "            </tr>\n"
                    + "          </table>\n"
                    + "          <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
                    + "          </table>\n"
                    + "          <table width=\"100%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" style=\"margin-bottom:15px;\">\n"
                    + "            <tr>\n"
                    + "              <td align=\"left\" valign=\"middle\" style=\"padding:15px; font-family:Arial, Helvetica, sans-serif;\">\n"
                    + "              <div align=\"right\" style=\"font-size:25px;  color:#468847;\"> Comprado por </div> <div  align=\"right\" style=\"font-size:36px; color:#468847;\">" + monto + " U$S! </div>\n"
                    + "              </td>\n"
                    + "            </tr>\n"
                    + "          </table>\n";
            if (calificacion)
            {
                cuerpo +=
                  "          <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style=\"margin-bottom:10px;\">\n"
                + "            <tr>\n"
                + "              <td align=\"left\" valign=\"middle\" style=\"padding:15px; background-color:#f89406 ; font-family:Arial, Helvetica, sans-serif;\"><div style=\"font-size:20px; color:#ffe77b;\">Calificaciones</div>\n"
                + "                <div style=\"font-size:13px; color:#000;\">Te invitamos a calificar la compra en nuestra página web. <br>\n"
                + "                  <br>\n"
                + "                  <a href=\"" + linkCalif + "\" style=\"color:#000; text-decoration:underline;\">CLICK AQUI</a> PARA CALIFICAR LA COMPRA </div></td>\n"
                + "            </tr>\n"
                + "          </table>\n";
            };
            cuerpo += "     <table width=\"100%\" border=\"0\" style=\"background-color: burlywood;\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">\n"
             + "            <tr>\n"
             + "              <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"padding:10px;\"><table width=\"75%\" border=\"0\" cellspacing=\"0\" cellpadding=\"4\">\n"
             + "                <tr>\n"
             + "                  <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:14px; color:#000000;\"><b>Seguinos en</b></td>\n"
             + "                </tr>\n"
             + "                <tr>\n"
             + "                  <td align=\"left\" valign=\"top\" style=\"font-family:Verdana, Geneva, sans-serif; font-size:12px; color:#000000;\"><table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n"
             + "                    <tr>\n"
             + "                <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><b>Facebook: </b> <a href=\"https://www.facebook.com\" style=\"color:#564319; text-decoration:none;\">Chebay FB</a><br>\n"
             + "                <br>\n"
             + "                 </tr>\n"
             + "                  </table></td>\n"
             + "                </tr>\n"
             + "              </table></td>\n"
             + "              <td width=\"50%\" align=\"left\" valign=\"middle\" style=\"color:#564319; font-size:11px; font-family:Arial, Helvetica, sans-serif; padding:10px;\"><b>Fecha:</b> " + fecha + "<br>\n"
             + "                <b>Atencion al cliente: </b> <a href=\"mailto:chebaysend@gmail.com\" style=\"color:#564319; text-decoration:none;\">chebaysend@gmail.com</a><br>\n"
             + "                <br>\n"
             + "                <b>Pagina web: </b><br>\n"
             + "Chebay: <a href=\"" + linkWeb + "\" target=\"_blank\"  style=\"color:#564319; text-decoration:none;\"> " + linkWeb + "</a></td>\n"
             + "            </tr>\n"
             + "          </table></td>\n"
             + "      </tr>\n"
             + "      </table>\n"
             + "    <br>\n"
             + "    <br></td>\n"
             + "  </tr>\n"
             + "</table>\n"
             + "</body>\n"
             + "</html>";

            return cuerpo;
        }

    }
}