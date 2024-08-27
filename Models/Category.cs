using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Shop.Models
{
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;

		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public int Popularity { get; set; } // For ranking categories

		public bool IsActive { get; set; } = true; // For soft delete

		public string Tag { get; set; } = string.Empty; // Tagging categories with keywords

		public decimal AverageProductPrice { get; set; } // Average price of products in this category

		[JsonIgnore]
		public virtual ICollection<Product>? Products { get; set; }
	}
}
