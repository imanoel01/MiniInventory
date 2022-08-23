using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Application.CQRS.Account.Commands
{
    public class CreateUserCommand
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
        }
        public class CreateUserCommandHandler : IRequestHandler<Command, Response>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private RoleManager<IdentityRole> roleManager;
            private readonly IConfiguration _configuration;

            public CreateUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
            {
                this.userManager = userManager;
                this.roleManager = roleManager;
                _configuration = configuration;
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
                    UserName = request.Username
                };
                var result = await userManager.CreateAsync(user, request.Password);
                if (!result.Succeeded)
                    return new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." };

                return new Response { Status = "Success", Message = "User created successfully!" };
            }
        }
    }
}