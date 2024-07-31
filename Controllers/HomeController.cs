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

        [HttpGet("TestUser")]
        [Authorize(Roles = "User")]
        public IActionResult TestUser()
        {
            return Ok(new { message = "Test User API" });
        }

        [HttpGet("Test Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdmin()
        {
            return Ok(new { message = "Test Admin API" });
        }

    }
}
