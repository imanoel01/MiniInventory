using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using AutoMapper;
using Application.CQRS.Roles.Queries;

namespace Application.MappingProfiles
{
    public class RoleMappingProfile:Profile
    {
       public RoleMappingProfile()
       {
        //  CreateMap<AddRole.Command, IdentityRole>()
        //   .ForMember(dest => dest.NormalizedName, opt => opt.MapFrom(src => src.Name.ToUpper()));

            CreateMap<PermissionModel, Application.CQRS.Roles.Queries.GetRolePermission.Model>();
       }
    }
}