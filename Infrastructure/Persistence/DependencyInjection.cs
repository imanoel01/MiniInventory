using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Persistence
{
    public static class DependencyInjection
    {
         public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
             
           services.AddScoped<IAppDataContext,AppDataContext>();
        }
    }
}