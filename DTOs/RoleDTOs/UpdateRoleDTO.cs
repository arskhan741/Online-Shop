using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.RoleDTOs
{
    public class UpdateRoleDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
    }
}
