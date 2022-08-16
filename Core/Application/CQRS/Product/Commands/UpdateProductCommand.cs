using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.CQRS.Commands
{
    public class UpdateProductCommand:IRequest<long>
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }
        public long CategoryId { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, long>
        {
            private IAppDataContext _context;

            public UpdateProductCommandHandler(IAppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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