using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.Roles.Queries
{
    public static class GetRole
    {
        public class Query : IRequest<Response>
        {

            [JsonIgnore]
            public Guid Id { get; set; }
        }
        public class Response
        {
            public Guid Id { get; set; }
            public string Role { get; set; }

        }

        public class GetRoleHandler : IRequestHandler<Query, Response>
        {
            private readonly AppDataContext _context;

            public GetRoleHandler(AppDataContext appDataContext)
            {
                _context = appDataContext;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var item = await (from role in _context.Roles.Where(x => x.Id == request.Id.ToString())
                                  select new Response
                                  {
                                      Id = Guid.Parse(role.Id),
                                      Role = role.Name
                                  }).SingleOrDefaultAsync();

                return item;
            }
        }

    }
}