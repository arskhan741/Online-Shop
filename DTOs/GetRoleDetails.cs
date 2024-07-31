using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs
{
    public class GetRoleDetails
    {
        [Required]
        public int Id { get; set; }
    }
}
