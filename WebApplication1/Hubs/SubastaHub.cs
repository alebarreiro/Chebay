using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Script.Serialization;
using System.Diagnostics;
using DataAccessLayer;
using Shared.Entities;

namespace WebApplication1.Hubs
{
    public class SubastaHub : Hub
    {
        IDALSubasta controladorSubasta = new DALSubastaEF();

        public void PlaceNewBid(int productId, int newBid, bool _esFinal, string userId, string tienda)
        {
            try
            {
                Oferta o = new Oferta
                {
                    esFinal = _esFinal,
                    monto = newBid,
                    ProductoID = productId,
                    UsuarioID = userId
                };

                controladorSubasta.OfertarProducto(o, tienda);
                Clients.All.newBidPosted(productId, newBid, userId);
            }
            catch(Exception e)
            {

            }
        }

        public void BuyAuction(int productId, int monto, string userId, string tienda)
        {
            try
            {
                Compra c = new Compra
                {
                    monto = monto,
                    fecha_compra = DateTime.Now,
                    ProductoID = productId,
                    UsuarioID = userId
                };
                controladorSubasta.AgregarCompra(c, tienda);
                Oferta o = new Oferta
                {
                    esFinal = true,
                    monto = monto,
                    ProductoID = productId,
                    UsuarioID = userId
                };
                controladorSubasta.OfertarProducto(o, tienda);
                Clients.All.newBuyPosted(productId, monto, userId);
            }
            catch (Exception e)
            {

            }
        }
    }
}