using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Persistence;

namespace Application.CQRS.Product.Commands
{
    public class DeleteProductCommand
    {

        public class Command : IRequest<long>
        {
            public long Id { get; set; }
        }
        public class DeleteProductCommandHandler : IRequestHandler<Command, long>
        {
            private readonly AppDataContext _context;

            public DeleteProductCommandHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(Command request, CancellationToken cancellationToken)
            {
                var product = await _context.Products.FindAsync(request.Id);

                if (product == null)
                    return default;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return product.Id;


            }
        }
    }
}