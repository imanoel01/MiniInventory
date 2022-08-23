using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;
using Application.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace MiniInventoryApi.Authorizations
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = false)]
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permissions permission) : base(permission.GetStringValue())
        {
        }
    }
}