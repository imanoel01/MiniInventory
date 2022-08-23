using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Application.Common;
using Application.Enums;
using Application.Extensions;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.CQRS.Roles.Queries;

public static class GetRolePermission
{
    public class Query : IRequest<List<GroupedModel>>
    {
        [JsonIgnore]
        public Guid RoleId { get; set; }
    }

    public class Model
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public Permissions Permission { get; set; }
        public List<Permissions> Dependencies { get; set; }
        public bool Granted { get; set; }
    }

    public class GroupedModel
    {
        public string GroupName { get; set; }
        public List<Model> Permissions { get; set; }

    }

    public class Response : List<GroupedModel>
    {
    }

 
    public class GetRolePermissionHandler : IRequestHandler<Query, List<GroupedModel>>
    {
        private readonly IPermissionHelper _permissionHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public GetRolePermissionHandler(IPermissionHelper permission, RoleManager<IdentityRole> roleManager,IMapper mapper)
        {
            _permissionHelper = permission;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<List<GroupedModel>> Handle(Query request, CancellationToken cancellationToken)
        {
              var allPermissions = _mapper.Map<List<Model>>(_permissionHelper.GetAllPermissions());

                var role = await _roleManager.FindByIdAsync(request.RoleId.ToString());
                var claims = await _roleManager.GetClaimsAsync(role);
                var allClaimValues = allPermissions.Select(a => a.Permission.GetStringValue()).ToList();
                var roleClaimValues = claims.Select(a => a.Value).ToList();
                var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

                foreach (var item in allPermissions)
                {
                    if (authorizedClaims.Contains(item.Permission.GetStringValue()))
                    {
                        item.Granted = true;
                    }
                }

                var grouped = allPermissions.GroupBy(x => x.GroupName).Select(x => new GroupedModel
                {
                    GroupName = x.Key,
                    Permissions = x.ToList()
                }).ToList();

                return grouped;
        }
    }

}
