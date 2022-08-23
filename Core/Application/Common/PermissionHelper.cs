using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Application.Attributes;
using Application.Enums;

namespace Application.Common
{

  public interface IPermissionHelper
    {
        List<PermissionModel> GetAllPermissions();

    }



    public class PermissionHelper :IPermissionHelper
    {
         public PermissionHelper()
        {
        }

        public List<PermissionModel> GetAllPermissions()
        {
            var result = new List<PermissionModel>();
            var enumType = typeof(Permissions);
            foreach (var permissionName in Enum.GetNames(enumType))
            {
                var member = enumType.GetMember(permissionName);

                //This allows you to obsolete a permission and it won't be shown as a possible option, but is still there so you won't reuse the number
                var obsoleteAttribute = member[0].GetCustomAttribute<ObsoleteAttribute>();
                if (obsoleteAttribute != null)
                    continue;

                //If there is no DisplayAttribute then the Enum is not used
                var displayAttribute = member[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttribute == null)
                    continue;

                var dependencies = member[0].GetCustomAttribute<PermissionDependencyAttribute>();
                var permDependencies = new List<Permissions>();
                if (dependencies != null && dependencies.Dependencies.Count > 0)
                {
                    foreach (var item in dependencies.Dependencies)
                    {
                        permDependencies.Add(item);
                    }
                }

                var permission = (Permissions)Enum.Parse(enumType, permissionName, false);

                result.Add(new PermissionModel
                {
                    GroupName = displayAttribute.GroupName,
                    Description = displayAttribute.Description,
                    Permission = permission,
                    Dependencies = permDependencies
                });
            }

            return result;
        
        }
    }
}