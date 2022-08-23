using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.Enums;
using Application.Extensions;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.CQRS.Roles.Commands
{
    public static class AddRole
    {
        public class Command : IRequest<Response>
        {
            

            [Required(ErrorMessage = "Name is required")]
            public string Name { get; set; }

            public List<Permissions> Permissions { get; set; }
        }

        public class Response : BasicActionResult
        {
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

        public class AddRoleHandler : IRequestHandler<Command, Response>
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly IConfiguration _configuration;

            public AddRoleHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _configuration = configuration;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    if (await _roleManager.RoleExistsAsync(request.Name))
                    {
                        return new Response("Role name exists");
                    }
                    var newRole = new IdentityRole { Name = request.Name };
                    await _roleManager.CreateAsync(newRole);

                    foreach (var item in request.Permissions)
                    {
                        await _roleManager.AddClaimAsync(newRole, new Claim(Constants.PermissionClaim, item.GetStringValue()));
                    }
                    return new Response(HttpStatusCode.Created);
                }
                catch (System.Exception ex)
                {
                    return new Response(HttpStatusCode.BadRequest);
                }



            }
        }
    }
}