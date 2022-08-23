using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Enums;

namespace Application.Common
{
    public class PermissionModel
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public Permissions Permission { get; set; }
        public List<Permissions> Dependencies { get; set; }
    }
}