using Microsoft.AspNetCore.Authorization;

namespace Online_Shop.Permissions
{
    public class HasPermissionAttribute : AuthorizeAttribute
    {
        public HasPermissionAttribute(Permission permission) 
            : base(policy: permission.ToString())
        {

        }
    }
}
