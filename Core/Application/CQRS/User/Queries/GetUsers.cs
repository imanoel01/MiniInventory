using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.User.Queries
{
    public static class GetUsers
    {
        public class Query : IRequest<List<Response>>
        {

        }

        public class Response
        {
            public string Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public bool EmailConfirmed { get; set; }
            public string Role { get; set; }
            public string PhoneNumber { get; set; }
            public bool TwoFactor { get; set; }

            public DateTime DateCreated { get; set; }
        }

        public class GetUsersHandler : IRequestHandler<Query, List<Response>>
        {
            public readonly AppDataContext _context;

            public GetUsersHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<List<Response>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = await(from user in _context.Users
                                   from userrole in _context.UserRoles.Where(ur => ur.UserId == user.Id).DefaultIfEmpty()
                                    from role in _context.Roles.Where(r => r.Id == userrole.RoleId).DefaultIfEmpty()
                                   select new Response
                                   {
                                       Id = user.Id,
                                       Email = user.Email,
                                       EmailConfirmed = user.EmailConfirmed,
                                       FirstName = user.Firstname,
                                       LastName = user.Lastname,
                                      Role = role.Name,
                                       PhoneNumber = user.PhoneNumber,
                                       TwoFactor = user.TwoFactorEnabled,
                                   }).ToListAsync();

                return query;
            }
        }
    }
}