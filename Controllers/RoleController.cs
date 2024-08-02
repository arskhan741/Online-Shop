using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Online_Shop.Contracts;
using Online_Shop.DTOs;
using Online_Shop.Models;

namespace Online_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RoleController> _logger;
        private readonly IMapper _mapper;

        public RoleController(IRoleService roleService, ILogger<RoleController> logger, IMapper mapper)
        {
            _roleService = roleService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(CreateRoleDTO createRoleDTO)
        {
            var role = _mapper.Map<Role>(createRoleDTO);

            await _roleService.CreateAsync(role);

            return CreatedAtAction(nameof(GetRole), new { message = "Role Added" ,id = role.Id, name = role.Name });
        }

        [HttpGet("GetRole")]
        public async Task<ActionResult<GetRoleDetails>> GetRole(int roleId)
        {
            var role = await _roleService.GetAsync(roleId);

            if (role == null) throw new NullReferenceException($"Role for {roleId} is null");

            var roleDetailsDTO = _mapper.Map<GetRoleDetails>(role);

            return Ok(roleDetailsDTO);
        }

        [HttpPut("UpdateRole")]
        public async Task<ActionResult> UpdateRole(int roleId, UpdateRoleDTO updateRoleDTO)
        {
            var role = await _roleService.GetAsync(roleId);

            _mapper.Map(updateRoleDTO, role);   //source, result

            if (role == null) return BadRequest($"Role for {roleId} is null");

            try
            {
                await _roleService.UpdateAsync(role);
                return Ok(new { message = $"role updated for role id = {role.Id}" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            await _roleService.DeleteAsync(roleId);

            return NoContent();
        }


        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<List<GetRoleDetails>>> GetAllRoles()
        {
            var roles = await _roleService.GetAllAsnyc();

            var roleDetails = _mapper.Map<List<GetRoleDetails>>(roles);

            return Ok(roleDetails);
        }

    }
}
