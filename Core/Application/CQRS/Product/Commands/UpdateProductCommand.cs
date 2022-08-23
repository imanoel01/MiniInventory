using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.CQRS.Product.Commands
{
    public class UpdateProductCommand
    {
        
public class Command:IRequest<long>
{

    public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
}

        public class UpdateProductCommandHandler : IRequestHandler<Command, long>
        {
            private AppDataContext _context;

            public UpdateProductCommandHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
               // throw new NotImplementedException();
               var product =await _context.Products.FindAsync(request.Id);
               if(product ==null)
               return default;
               product.Name = request.Name;
               product.Description = request.Description;
               product.Quantity= request.Quantity;
               product.Price = request.Price;
product.CategoryId = request.CategoryId;
               await _context.SaveChangesAsync();
               return product.Id;
            }
        }
    }
}