using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application
{
    public interface IAppDataContext
    {
        DbContext Instance { get; }
        DbSet<Product> Products {get;set;}
        Task<int> SaveChangesAsync();
    }
}