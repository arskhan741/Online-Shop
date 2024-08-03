using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.CategoryDTOs
{
    public class GetCategoryDetailsDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public String Name { get; set; } = string.Empty;
        [Required]
        public String Description { get; set; } = string.Empty;
    }
}
