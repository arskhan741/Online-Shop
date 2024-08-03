using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string NewName { get; set; } = string.Empty;
        [Required]
        public string NewDescription { get; set; } = string.Empty;
    }
}
