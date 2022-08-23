using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using MiniInventoryApi.Authorizations;

namespace MiniInventory.Services
{
    public static class ServiceCollectionExtension
    {
        public static void RegisterOtherServices(this IServiceCollection services, IConfiguration configuration)
        {
             services.AddCors(c =>
            {
                c.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


   //Register the Permission policy handlers
            //    services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

        }
    }
}