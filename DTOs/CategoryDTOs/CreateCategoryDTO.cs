using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        public String Name { get; set; } = String.Empty;
        [Required]
        public String Description { get; set; } = String.Empty;
    }
}
