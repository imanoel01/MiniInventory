using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection service)
        {
            service.AddMediatR(Assembly.GetExecutingAssembly());

            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddScoped<IPermissionHelper,PermissionHelper>();
        }
    }
}