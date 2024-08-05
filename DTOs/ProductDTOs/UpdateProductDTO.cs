using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.ProductDTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string NewName { get; set; } = string.Empty;
        [Required]
        public string NewDescription { get; set; } = string.Empty;
        [Required]
        public int CategoryId { get; set; }

    }
}
