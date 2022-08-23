using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.Extensions
{
    public static class PermissionExtensions
    {
        public static string GetStringValue(this Permissions permission)
        {
            // return the integer enum value as a string
            return ((int)permission).ToString();
        }

    }
}