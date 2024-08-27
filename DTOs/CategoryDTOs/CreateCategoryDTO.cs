using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.CategoryDTOs
{
	public class CreateCategoryDTO
	{
		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Description { get; set; } = string.Empty;

		public bool IsActive { get; set; } = true; // Optional for category creation

		public string Tag { get; set; } = string.Empty; // Optional tag field

		public int Popularity { get; set; } // For ranking categories

		public decimal AverageProductPrice { get; set; } // Average price of products in this category

	}
}
