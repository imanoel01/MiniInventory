using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    [Index(nameof(Name))]
    public class Product
    {
        [Key]
        public long Id { get; set; } 
        public string  Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}