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

            Oferta o = new Oferta { 
                esFinal = _esFinal,
                monto = newBid,
                ProductoID = productId,
                UsuarioID = userId
            };

            controladorSubasta.OfertarProducto(o, tienda);
            Clients.All.newBidPosted(productId, newBid, userId);
        }

    }
}