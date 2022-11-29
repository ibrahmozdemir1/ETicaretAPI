using ETicaretAPI.Application.Repositories.Customers;
using ETicaretAPI.Application.Repositories.Orders;
using ETicaretAPI.Application.Repositories.Products;
using ETicaretAPI.Application.Services;
using ETicaretAPI.Infrastucture.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastucture
{
    public static class InfrastructureServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();

        }
    }
}
