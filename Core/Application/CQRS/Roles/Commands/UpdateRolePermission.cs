using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common;
using Application.Enums;
using Application.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Roles.Commands
{
    public static class UpdateRolePermission 
    {
        

        public class Command : IRequest<Response>
        {
            [Required(ErrorMessage = "Role Id is required")]
            public Guid? RoleId { get; set; }

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

       
        public class UpdateRolePermissionHandler : IRequestHandler<Command, Response>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public UpdateRolePermissionHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {

                try
                {
                    var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
                    if (role == null)
                    {
                        return new Response("Role does not exist");
                    }
                    var existingClaims = await _roleManager.GetClaimsAsync(role);
                    var newPermissions = request.Permissions.Select(x => x.GetStringValue());
                    var existingPermissions = existingClaims.Select(x => x.Value).ToList();
                    var newlyAdded = newPermissions.Except(existingPermissions).ToList();
                    var removed = existingPermissions.Except(newPermissions).ToList();

                    foreach (var item in newlyAdded)
                    {
                        await _roleManager.AddClaimAsync(role, new Claim(Constants.PermissionClaim, item));
                    }

                    foreach (var item in removed)
                    {
                        await _roleManager.RemoveClaimAsync(role, existingClaims.Single(x => x.Value == item));
                    }
                    return new Response(HttpStatusCode.NoContent);
                }
                catch (System.Exception ex)
                {

                    return new Response(HttpStatusCode.BadRequest);
                }

            }
        }
    }
}