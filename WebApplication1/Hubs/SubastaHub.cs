using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace WebApplication1.Hubs
{
    public class SubastaHub : Hub
    {

        public void PlaceNewBid(int productId, int newBid)
        {
            Clients.All.newBidPosted(productId, newBid);
        }

    }
}