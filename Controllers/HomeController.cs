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

        [HttpGet("TestAdminRole")]
        [Authorize(Roles = "Admin")]
        public IActionResult TestAdminRole()
        {
            return Ok(new { message = "Test Admin API" });
        }


        [HttpGet("TestManagerRole")]
        [Authorize(Roles = "Manager")]
        public IActionResult TestManagerRole()
        {
            return Ok(new { message = "Test Manager API" });
        }

        [HttpGet("TestMemberRole")]
        [Authorize(Roles = "Member")]
        public IActionResult TestMemberRole()
        {
            return Ok(new { message = "Test Member API" });
        }

        [HttpGet("TestUserRole")]
        [Authorize(Roles = "User")]
        public IActionResult TestUserRole()
        {
            return Ok(new { message = "Test User Policy API" });
        }

        [HttpGet("TestAdminPolicy")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult TestAdminPolicy()
        {
            return Ok(new { message = "Test Admin Policy API" });
        }

        [HttpGet("TestManagerPolicy")]
        [Authorize(Policy = "ManagerPolicy")]
        public IActionResult TestManagerPolicy()
        {
            return Ok(new { message = "Test Manager Policy API" });
        }

        [HttpGet("TestMemberPolicy")]
        [Authorize(Policy = "MemberPolicy")]
        public IActionResult TestMemberPolicy()
        {
            return Ok(new { message = "Test Member Policy API" });
        }

        [HttpGet("TestUserPolicy")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult TestUserPolicy()
        {
            return Ok(new { message = "Test User Policy API" });
        }
    }
}
