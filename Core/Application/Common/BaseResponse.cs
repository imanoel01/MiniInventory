using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common
{

    public class BaseResponse
    {
        public string ErrorMessage { get; set; }
    }

    public class UnauthorizedResponse : BaseResponse
    {
        public bool IsExpired { get; set; }
    }
}
