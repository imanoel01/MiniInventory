using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.Roles.Queries
{
    public static class GetRoles
    {

        public class Query : IRequest<List<Response>>
        {

        }

        public class Response : BasicActionResult
        {

            public Guid Id { get; set; }
            public string Role { get; set; }
        }

        public class GetRolesHandler : IRequestHandler<Query, List<Response>>
        {
            private readonly AppDataContext _context;

            public GetRolesHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await (from role in _context.Roles
                                  select new Response
                                  {
                                      Id = Guid.Parse(role.Id),
                                      Role = role.Name
                                  }).ToListAsync();

                return item;
            }
        }
    }
}