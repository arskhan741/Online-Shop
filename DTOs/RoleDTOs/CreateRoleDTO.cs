using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.RoleDTOs
{
    public class CreateRoleDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
