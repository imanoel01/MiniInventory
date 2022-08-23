using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;
using Persistence;

namespace Application.CQRS.Queries
{
    public class GetProductByIdQuery
    {

        public class Query : IRequest<Domain.Models.Product>
        {
            public long Id { get; set; }
        }
        public class GetProductByIdQueryHandler : IRequestHandler<Query, Domain.Models.Product>
        {
            private readonly AppDataContext _context;

            public GetProductByIdQueryHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<Domain.Models.Product> Handle(Query request, CancellationToken cancellationToken)
            {
                //throw new NotImplementedException();

                var product = await _context.Products.FindAsync(request.Id);
                if (product == null)
                    return null;

                return product;
            }
        }
    }
}