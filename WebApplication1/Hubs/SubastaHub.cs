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

        public void PlaceNewBid(int productId, int newBid, bool _esFinal, string userId)
        {
            IDALSubasta controladorSubasta = new DALSubastaEF();

            Oferta o = new Oferta { 
                esFinal = _esFinal,
                monto = newBid,
                ProductoID = productId,
                UsuarioID = userId
            };
            controladorSubasta.OfertarProducto(o, "TestURL");
            Clients.All.newBidPosted(productId, newBid, userId);
        }

    }
}