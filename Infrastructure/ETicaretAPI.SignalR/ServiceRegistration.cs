﻿using ETicaretAPI.Application.Abstractions.Hubs;
using ETicaretAPI.SignalR.HubServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.SignalR
{
    public static class ServiceRegistration
    {
        public static void AddSignalRServices(this IServiceCollection collection)
        {
            collection.AddScoped<IProductHubService, ProductHubService>();
            collection.AddScoped<IOrderHubService, OrderHubService>();
            collection.AddSignalR();
        }
    }
}
