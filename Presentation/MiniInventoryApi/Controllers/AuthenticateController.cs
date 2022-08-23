using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Account.Commands;
using Application.CQRS.Account.Queries;
using Application.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MiniInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthenticateController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        // public ActionResult<IEnumerable<string>> Get()
        // {
        //     return new string[] { "value1", "value2" };
        // }

        // [HttpGet("{id}")]
        // public ActionResult<string> Get(int id)
        // {
        //     return "value";
        // }

        [HttpPost]
        public async Task<IActionResult> Post(CreateUserCommand.Command value)
        {
            return Ok(await _mediator.Send(value));
        }
        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> Post(CreateAdminCommand.Command value)
        {
            return Ok(await _mediator.Send(value));
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(Login.Query value)
        {
            return Ok(await _mediator.Send(value));
        }

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