using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Application.Common;

namespace Application.CQRS.Queries
{
    public static class Login
    {

        public class Query : IRequest<Response>
        {

            [Required(ErrorMessage = "User Name is required")]
            public string Username { get; set; }

            [Required(ErrorMessage = "Password is required")]
            public string Password { get; set; }
        }




        public class Response : BasicActionResult
        {
            public string UserId { get; set; }
            public Guid CompanyId { get; set; }
            public string Username { get; set; }
            public string CompanyLogoId { get; set; }
            public bool EmailConfirmed { get; set; }
            public bool Require2FA { get; set; }
            //  public CompanySubscriptionDTO Subscription { get; set; }//CompanySubscriptionDTO
            public string Referralcode { get; set; }
            public string Token { get; set; }

            // [JsonIgnore] // refresh token is returned in http only cookie
            public string RefreshToken { get; set; }

            public DateTime? RefreshTokenExpiration { get; set; }

            public Response()
            {
                Status = HttpStatusCode.OK;
            }

            public Response(string message)
            {
                ErrorMessage = message;
                Status = HttpStatusCode.BadRequest;
            }
        }

        public class LoginQueryHandler : IRequestHandler<Query, Response>
        {
            private readonly UserManager<ApplicationUser> userManager;
            private readonly RoleManager<IdentityRole> roleManager;
            private readonly IConfiguration _configuration;

            public LoginQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
            {

                this.userManager = userManager;
                this.roleManager = roleManager;
                _configuration = configuration;
            }
            public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await userManager.FindByNameAsync(request.Username);
                if (user != null && await userManager.CheckPasswordAsync(user, request.Password))
                {
                    var userRoles = await userManager.GetRolesAsync(user);

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                    foreach (var userRole in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                    var token = new JwtSecurityToken(
                        issuer: _configuration["JWT:ValidIssuer"],
                        audience: _configuration["JWT:ValidAudience"],
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return new Response
                    {
                        UserId = user.Id,
                        //CompanyId = user.CompanyId,
                        EmailConfirmed = user.EmailConfirmed,
                        Require2FA = user.TwoFactorEnabled,
                        Username = user.UserName,
                        // CompanyLogoId = company.LogoId,
                        //  Referralcode = company.Ref_ReferralCode == null ? string.Empty : company.Ref_ReferralCode,
                        // Subscription = subscription,
                        //Token = token,
                        //RefreshToken = refreshToken?.Token,
                        // RefreshTokenExpiration = refreshToken?.Expires,
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        RefreshTokenExpiration = token.ValidTo
                    };
                }
                return new Response($"Incorrect credentials");
            }
        }

    }
}