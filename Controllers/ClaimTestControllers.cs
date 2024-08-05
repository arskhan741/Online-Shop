using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Shop.Controllers
{
    [ApiController]
    [Route("api/ClaimTest")]
    public class ClaimTestControllers : ControllerBase
    {
        [HttpGet("TestClaimsTest1Role")]
        [Authorize(Policy = "ClaimsTest1")]
        public IActionResult TestClaimsTest1Role()
        {
            return Ok(new { message = "Test ClaimsTest1 API" });
        }




    }
}
