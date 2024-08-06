using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Shop.Controllers
{
    [ApiController]
    [Route("api/ClaimTest")]
    public class ClaimTestControllers : ControllerBase
    {
        [HttpGet("TestAdminClaimsRole")]
        [Authorize(Policy = "Claims_Admin")]
        public IActionResult TestAdminClaimsRole()
        {
            return Ok(new { message = "Test AdminClaims API" });
        }

        [HttpGet("TestCombinedPolicy")]
        [Authorize(Policy = "CombinedPolicy")]
        public IActionResult TestCombinedPolicy()
        {
            return Ok(new { message = "Test Combined Policy API"});
        }
    }
}
