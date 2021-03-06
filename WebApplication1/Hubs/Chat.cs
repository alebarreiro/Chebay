﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Diagnostics;

namespace WebApplication1.Hubs
{
    public class Chat : Hub
    {
        public void sendBroadcast(string message)
        {
            Clients.All.receiveBroadcast(message);
        }

        // iniciar sesion
        public async Task JoinGroup(string group)
        {
            //pensar en nombre de usuario Request.authenticated
            await Groups.Add(Context.ConnectionId, group);
        }

        public void sendGroup(string group, string user, string mensaje)
        {
            Clients.Group(group).receiveGroup(user + ": " + mensaje);
        }

    }
}