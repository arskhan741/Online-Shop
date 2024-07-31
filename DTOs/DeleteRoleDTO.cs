using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs
{
    public class DeleteRoleDTO
    {
        [Required]
        public int Id { get; set; }
    }
}
