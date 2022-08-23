using Application.Enums;

namespace Application.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class PermissionDependencyAttribute : Attribute
    {
        public Permissions Permission;
        public List<Permissions> Dependencies;

        public PermissionDependencyAttribute(params Permissions[] permissions)
        {
            Dependencies = permissions.ToList();
            Permission = Dependencies.First();
            Dependencies.RemoveAt(0); // remove main permission from dependencies list
        }
    }
}