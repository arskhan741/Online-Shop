using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.CategoryDTOs
{
    public class DeleteCategoryDTO
    {
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; } = string.Empty;
    }
}
