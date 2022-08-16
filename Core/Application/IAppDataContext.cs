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
        DbSet<Product> Products {get;set;}
        Task<int> SaveChangesAsync();
    }
}