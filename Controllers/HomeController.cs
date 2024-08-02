using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Online_Shop.Controllers
{
    [ApiController]
    [Route("api/fruits")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Test Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok(new { message = "Test Admin API" });
        }


        [HttpGet("TestManager")]
        [Authorize(Roles = "Manager")]
        public IActionResult TestManager()
        {
            return Ok(new { message = "Test Manager API" });
        }

        [HttpGet("TestMember")]
        [Authorize(Roles = "Member")]
        public IActionResult TestMember()
        {
            return Ok(new { message = "Test Member API" });
        }

        [HttpGet("TestUserPolicy")]
        //[Authorize(Policy = "UserPolicy")]
        [Authorize(Roles = "User")]
        public IActionResult TestUserPolicy()
        {
            return Ok(new { message = "Test User Policy API" });
        }
    }
}
