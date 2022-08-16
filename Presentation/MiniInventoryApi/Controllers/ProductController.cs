using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MiniInventoryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetAllProductQuery()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _mediator.Send(new GetProductByIdQuery() { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductCommand model)
        {
            return Ok(await _mediator.Send(model));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UpdateProductCommand model)
        {
            if (id != model.Id)
                return BadRequest();
            return Ok( await _mediator.Send(model));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductCommand(){Id =id}));
        }
    }
}