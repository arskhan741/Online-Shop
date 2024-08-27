using Online_Shop.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_Shop.DTOs.ProductDTOs
{
    public class CreateProductDTO
    {
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Description { get; set; } = string.Empty;

		[Required]
		public int CategoryId { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int Stock { get; set; }

		public bool IsFeatured { get; set; } = false;

		public float Rating { get; set; } = 0; // Optional for product creation
	}

}
