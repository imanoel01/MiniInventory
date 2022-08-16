using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Category
    {
        public long Id { get; set; } 
        public string  Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}