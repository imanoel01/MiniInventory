using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.CQRS.User.Command
{
    public static class UpdateUser
    {
        public class Command : IRequest<Response>
        {
            [JsonIgnore]
            public Guid UserId { get; set; }
            [JsonIgnore]
            public Guid CompanyId { get; set; }

            [JsonIgnore]
            public Guid Id { get; set; }

            [Required(ErrorMessage = "First name is required")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Last name is required")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "Phone number is required")]
            public string PhoneNumber { get; set; }

            public bool EnableTwoFactorAuth { get; set; }
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

        public class UpdateUserHandler : IRequestHandler<Command, Response>
        {
            public readonly AppDataContext _context;

            public UpdateUserHandler(AppDataContext context)
            {
                _context = context;
            }
            public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == request.Id.ToString());
                    user.PhoneNumber = request.PhoneNumber;
                    user.Firstname = request.FirstName;
                    user.Lastname = request.LastName;
                    user.TwoFactorEnabled = request.EnableTwoFactorAuth;

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