using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;

namespace Application.CQRS.Queries
{
    public class GetProductByIdQuery :IRequest<Product>
    {
        public long Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
        {
            private readonly IAppDataContext _context;

            public GetProductByIdQueryHandler(IAppDataContext context)
            {
              _context = context;    
            }
            public async Task<Product> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {
                //throw new NotImplementedException();

                var product =await _context.Products.FindAsync(request.Id);
                if(product == null)
                return null;

                return product;
            }
        }
    }
}