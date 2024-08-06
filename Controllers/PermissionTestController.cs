using Microsoft.AspNetCore.Mvc;
using Online_Shop.Permissions;

namespace Online_Shop.Controllers
{
    public class PermissionTestController : ControllerBase
    {

        [HasPermission(Permission.ReadMember)]
        [HttpGet("CanReadPermissionTest")]
        public IActionResult CanReadPermissionTest()
        {
            return Ok(new {message = "Can Read Permission Test" });
        }
    }
}
