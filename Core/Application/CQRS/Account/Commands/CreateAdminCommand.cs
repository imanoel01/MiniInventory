using System.ComponentModel.DataAnnotations;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;
//using Persistence;

namespace Application.CQRS.Account.Commands
{
    public class CreateAdminCommand
    {
        public class Command : IRequest<Response>
        {
            [Required(ErrorMessage = "Firstname is required")]
            public string Firstname { get; set; }
            [Required(ErrorMessage = "Middlename is required")]
            public string Middlename { get; set; }
            [Required(ErrorMessage = "Lastname is required")]
            public string Lastname { get; set; }

            [Required(ErrorMessage = "User Name is required")]
            public string Username { get; set; }

            [EmailAddress]
            [Required(ErrorMessage = "Email is required")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
            public Guid? RoleId { get; set; }
        }
        public class CreateAdminCommandHandler : IRequestHandler<Command, Response>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IConfiguration _configuration;
            private readonly AppDataContext _context;

            public CreateAdminCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AppDataContext context)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                _configuration = configuration;
                _context = context;
            }

            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {


                var userExists = await userManager.FindByNameAsync(request.Username);
                if (userExists != null)
                    return new Response { Status = "Error", Message = "User already exists!" };

                ApplicationUser user = new ApplicationUser()
                {
                    Firstname = request.Firstname,
                    Middlename = request.Middlename,
                    Lastname = request.Lastname,
                    Email = request.Email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = request.Username,
                    RoleId = request.RoleId ==null? Guid.Empty :request.RoleId

                };
                
                var result = await userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." };


                var role = await _context.Roles.Where(x => x.Id == request.RoleId.ToString())
                                                                                       .Select(x => new { x.Id, x.Name }).SingleAsync();
                if (role != null)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                // if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                //     await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                // if (!await roleManager.RoleExistsAsync(UserRoles.User))
                //     await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // if (await roleManager.RoleExistsAsync(UserRoles.Admin))
                // {
                //     await userManager.AddToRoleAsync(user, UserRoles.Admin);
                // }

                return new Response { Status = "Success", Message = "User created successfully!" };
            }
        }
    }
}