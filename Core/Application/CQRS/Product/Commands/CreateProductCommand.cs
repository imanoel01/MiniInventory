using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Domain.Models;

namespace Application.CQRS.Commands
{
    public class CreateProductCommand : IRequest<long>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
        public long  CategoryId{ get; set; }
        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, long>
        {
            private readonly IAppDataContext _context;
            public CreateProductCommandHandler(IAppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(CreateProductCommand request, CancellationToken cancellationToken)
            {

                
                //throw new NotImplementedException();
                var product = new Product();
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