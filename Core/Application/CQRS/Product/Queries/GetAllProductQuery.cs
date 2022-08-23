using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.Product.Queries
{
    public class GetAllProductQuery 
    {
        public class Query : IRequest<IEnumerable<Domain.Models.Product>>
        {

        }
        public class GetAllProductQueryHandler : IRequestHandler<Query, IEnumerable<Domain.Models.Product>>
        {
            private readonly AppDataContext _context;

            public GetAllProductQueryHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Domain.Models.Product>> Handle(Query request, CancellationToken cancellationToken)
            {
                var products = await _context.Products.ToListAsync();
                if (products == null)
                    return null;

                return products.AsReadOnly();

            }
        }
    }
}