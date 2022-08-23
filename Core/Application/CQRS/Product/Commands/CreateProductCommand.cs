using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Models;
using Persistence;

namespace Application.CQRS.Product.Commands
{
    public class CreateProductCommand 
    {
        public class Command :IRequest<long>
        {
            public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
        public long  CategoryId{ get; set; }
        }

        public class CreateProductCommandHandler : IRequestHandler<Command, long>
        {
            private readonly AppDataContext _context;
            public CreateProductCommandHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {

                
                //throw new NotImplementedException();
                var product = new Domain.Models.Product();
                product.Name = request.Name;
                product.Description= request.Description;
                product.Price= request.Price;
                product.Quantity = request.Quantity;
                product.CategoryId = request.CategoryId;
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return product.Id;
            }
        }
    }
}