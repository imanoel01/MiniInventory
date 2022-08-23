using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Product.Commands;
using Application.CQRS.Product.Queries;
using Application.CQRS.Queries;
using Application.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MiniInventory.Services;
using MiniInventoryApi.Authorizations;

namespace MiniInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : MyControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator):base(mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [HasPermission(Permissions.ViewUsers)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllProductQuery.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _mediator.Send(new GetProductByIdQuery.Query() { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand.Command model)
        {
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateProductCommand.Command model)
        {
            if (id != model.Id)
                return BadRequest();
            return Ok( await _mediator.Send(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand.Command(){Id =id}));
        }
    }
}