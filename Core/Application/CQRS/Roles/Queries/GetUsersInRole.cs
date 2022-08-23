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
    public static class GetUserInRole
    {
        public class Query : IRequest<List<Response>>
        {
            public Guid RoleId { get; set; }
        }

        public class Response : BasicActionResult
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
        }

        public class GetUserInRoleHandler : IRequestHandler<Query, List<Response>>
        {
            public readonly AppDataContext _context;

            public GetUserInRoleHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await (from ur in _context.UserRoles
                              join u in _context.Users
                              on ur.UserId equals u.Id
                              where ur.RoleId==request.RoleId.ToString()
                              select new Response
                              {
                                  Id = u.Id,
                                  Firstname = u.Firstname,
                                  Lastname = u.Lastname,
                                  Email = u.Email
                              }).ToListAsync();
            }
        }
    }
}