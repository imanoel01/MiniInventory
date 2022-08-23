using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MiniInventory.Services
{
    public interface IMyControllerBase
    {
        // Guid? CompanyId { get; set; }
        // Guid? UserId { get; set; }
        // string Username { get; set; }
        IMediator Mediator { get; }
        //  ILogger Logger { get; }

    }

    public class MyControllerBase : ControllerBase, IMyControllerBase
    {
        public IMediator Mediator { get; }

        public MyControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected IActionResult CommandResponse(BasicActionResult result)
        {
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                var objectResult = new ObjectResult(new BaseResponse { ErrorMessage = result.ErrorMessage })
                {
                    StatusCode = (int)result.Status
                };

                return objectResult;
            }

            if (result.Status == HttpStatusCode.OK)
            {
                return Ok(result);
            }

            return StatusCode((int)result.Status, result);
        }


        protected IActionResult QueryResponse(object data)
        {
            if (data == null)
            {
                return NotFound("No record found");
            }

            var objectResult = new ObjectResult(data)
            {
                StatusCode = 200
            };

            return objectResult;
        }
    }
}