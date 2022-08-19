using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Common
{
    public class BasicActionResult
    {
          [JsonIgnore]
        public HttpStatusCode Status { get; set; }
        [JsonIgnore]
        public string ErrorMessage { get; set; }
        public string Message { get; set; }

        public BasicActionResult(string errorMessage)
        {
            ErrorMessage = errorMessage;
            Status = HttpStatusCode.BadRequest;
        }
        public BasicActionResult(string message, HttpStatusCode statusCode)
        {
            Message = message;
            Status = statusCode;
        }

        public BasicActionResult()
        {
            Status = HttpStatusCode.OK;
        }

        public BasicActionResult(HttpStatusCode status)
        {
            Status = status;
        }
    }
}