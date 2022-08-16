using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Queries
{
    public class GetAllProductQuery : IRequest<IEnumerable<Product>>
    {
        public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
        {
            private readonly IAppDataContext _context;

            public GetAllProductQueryHandler(IAppDataContext context)
            {
                _context = context;
            }
            public async Task<IEnumerable<Product>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                var products = await _context.Products.ToListAsync();
                if (products == null)
                    return null;

                return products.AsReadOnly();

            }
        }
    }
}