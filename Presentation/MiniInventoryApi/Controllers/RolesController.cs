using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Roles.Commands;
using Application.CQRS.Roles.Queries;
//using Application.CQRS.Roles.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Services;

namespace MiniInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : MyControllerBase
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator) : base(mediator)
        {
            _mediator = mediator;
        }
        [HttpGet()]
        public async Task<IActionResult> GetRoles()
        {
            return QueryResponse(await _mediator.Send(new GetRoles.Query() { }));
        }

        [HttpGet("{roleId}/users")]
        public async Task<IActionResult> GetUsersinRole(Guid roleId)
        {
            return QueryResponse(await _mediator.Send(new GetUserInRole.Query { RoleId = roleId }));
        }

        ///<summary>
        /// get role permissions
        ///</summary>
        [HttpGet("{roleId}/permissions")]
        public async Task<IActionResult> RolePermissions(Guid roleId)
        {

            return QueryResponse(await _mediator.Send(new GetRolePermission.Query { RoleId = roleId }));
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(AddRole.Command value)
        {
            return CommandResponse(await _mediator.Send(value));
        }

        [HttpPut("{roleId}/permissions")]
        public async Task<IActionResult> Put(Guid roleId, UpdateRolePermission.Command value)
        {
            if (roleId != value.RoleId)
                return BadRequest();
            return CommandResponse(await _mediator.Send(value));
        }

        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}