using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence
{
    public class AppDataContext : DbContext, IAppDataContext
    {
       protected readonly IConfiguration _configuration;

        public AppDataContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }
        public DbSet<Product> Products { get; set; }

        public async Task<int> SaveChangesAsync()
        {
          return await base.SaveChangesAsync();
        }
    }
}