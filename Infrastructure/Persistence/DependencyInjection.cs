using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Application;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

//using Microsoft.IdentityModel.Tokens; 

namespace Persistence
{
    public static class DependencyInjection
    {
         public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
             
        //    services.AddScoped<IAppDataContext,AppDataContext>();

        //     //For Identity
        //     services.AddIdentity<ApplicationUser,IdentityRole>()
        //     .AddEntityFrameworkStores<AppDataContext>()
        //     .AddDefaultTokenProviders();

        services.AddDbContext<AppDataContext>();
// services.AddScoped<IAppDataContext>(sp => sp.GetRequiredService<AppDataContext>());

//For Identity
            services.AddIdentity<ApplicationUser,IdentityRole>()
            .AddEntityFrameworkStores<AppDataContext>()
            .AddDefaultTokenProviders();

        }
    }
}