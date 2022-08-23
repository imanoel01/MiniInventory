using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.User.Command
{
    public static class UpdateUserRole
    {
        public class Command : IRequest<Response>
        {

            [JsonIgnore]
            public Guid RoleId { get; set; }

            [Required(ErrorMessage = "UserId is required")]
            public Guid? UserId { get; set; }

        }

        public class Response : BasicActionResult
        {
            public Response()
            {
                Status = HttpStatusCode.OK;
            }

            public Response(HttpStatusCode statusCode)
            {
                Status = statusCode;
            }

            public Response(string message)
            {
                ErrorMessage = message;
                Status = HttpStatusCode.BadRequest;
            }

        }

        public class UpdateUserRoleHandler : IRequestHandler<Command, Response>
        {
            private readonly AppDataContext _context;
            private readonly UserManager<ApplicationUser> _userManager;

            public UpdateUserRoleHandler(AppDataContext context, UserManager<ApplicationUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    var user = await _userManager.FindByIdAsync(request.UserId.Value.ToString());

                    if (user == null)
                    {
                        return new Response("User not found");
                    }

                    var exisitngRoles = await _userManager.GetRolesAsync(user);

                    if (exisitngRoles.Count > 0)
                    {
                        var existingRole = exisitngRoles.First();

                        await _userManager.RemoveFromRoleAsync(user, existingRole);

                    }


                    var role = await _context.Roles.Where(x => x.Id == request.RoleId.ToString())
                                                                        .Select(x => new { x.Id, x.Name }).SingleAsync();

                    await _userManager.AddToRoleAsync(user, role.Name);
                    user.RoleId = Guid.Parse(role.Id);
                    await _context.SaveChangesAsync();

                    return new Response(HttpStatusCode.Created);

                }
                catch (System.Exception ex)
                {
                    return new Response(HttpStatusCode.BadRequest);
                    //throw;
                }


            }
        }
    }
}