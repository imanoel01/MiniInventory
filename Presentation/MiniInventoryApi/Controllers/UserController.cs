using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Application.CQRS.User.Command;
using Application.CQRS.User.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Services;

namespace MiniInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : MyControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return QueryResponse(await _mediator.Send(new GetUsers.Query()));
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(Guid userId, UpdateUser.Command request)
        {
            request.Id = userId;
            return CommandResponse( await _mediator.Send(request));
        }

        [HttpPut("{roleId}/users")]

        [Consumes(MediaTypeNames.Application.Json)]

        public async Task<IActionResult> AddUserToRole([FromRoute]Guid roleId,[FromBody] UpdateUserRole.Command request)
        {
            request.RoleId = roleId;
            var result = await _mediator.Send(request);
            return CommandResponse(result);
        }

        // [HttpGet("{id}")]
        // public ActionResult<string> Get(int id)
        // {
        //     return "value";
        // }

        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}