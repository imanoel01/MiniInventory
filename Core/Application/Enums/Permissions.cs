using System.ComponentModel.DataAnnotations;
using Application.Attributes;

namespace Application.Enums
{
    public enum Permissions
    {
        
         //Users
        [Display(GroupName = "Users", Description = "Can view users")]
        ViewUsers = 1,
        [Display(GroupName = "Users", Description = "Can update users")]
        [PermissionDependency(UpdateUser, ViewUsers)]
        UpdateUser,
        [Display(GroupName = "Users", Description = "Can update users role")]
        [PermissionDependency(UpdateUserRole, ViewUsers)]
        UpdateUserRole,
        [Display(GroupName = "Users", Description = "Can delete users")]
        [PermissionDependency(DeleteUsers, ViewUsers)]
        DeleteUsers,
    }
}