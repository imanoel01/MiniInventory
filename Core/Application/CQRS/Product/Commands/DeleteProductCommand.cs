using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace Application.CQRS.Commands
{
    public class DeleteProductCommand : IRequest<long>
    {
        public long Id { get; set; }
        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, long>
        {
            private readonly IAppDataContext _context;

            public DeleteProductCommandHandler(IAppDataContext context)
            {
                _context = context;
            }
            public async Task<long> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
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