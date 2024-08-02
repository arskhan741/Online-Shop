using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Contracts;
using Online_Shop.Models;

namespace Online_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IUserRepository authService, ILogger<UserController> logger, RoleManager<IdentityRole> roleManager)
        {
            _userRepository = authService;
            _logger = logger;
            _roleManager = roleManager;
        }


        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");
                var (status, message) = await _userRepository.Login(model);
                if (status == 0)
                    return BadRequest(message);
                return Ok(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("Registeration")]
        public async Task<IActionResult> Register(RegistrationModel model, string enteredRole)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");


                enteredRole.ToLower();

                var (status, message) = await _userRepository.CreateUser(model, enteredRole);


                if (status == 0)
                {
                    return BadRequest(message);
                }

                return CreatedAtAction(nameof(Register), new { message = $"{model.Name} {message}" });

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(RegistrationModel model, string newPassword)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid payload");

                var (status, message) = await _userRepository.UpdateUser(model, newPassword);

                if (status == 0) return BadRequest(message);

                return CreatedAtAction(nameof(UpdateUser), new { message = message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            var (status, message) = await _userRepository.DeleteUser(email);

            if (status == 0) return BadRequest(message);

            return Ok(message);
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(string email)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            var user = await _userRepository.GetUser(email);

            return Ok(user);
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid payload");

            var users = await _userRepository.GetAllUser();

            return Ok(users);
        }

    }
}
