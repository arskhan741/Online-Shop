using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Online_Shop.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; } = string.Empty;

		[Required]
		public string Description { get; set; } = string.Empty;

		[Required]
		public int CategoryId { get; set; }

		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public DateTime UpdatedOn { get; set; } = DateTime.Now;

		public decimal Price { get; set; } // Price of the product

		public int Stock { get; set; } // Number of units in stock

		public bool IsFeatured { get; set; } = false; // Mark as featured product

		public float Rating { get; set; } // Product rating (1-5 scale)

		[JsonIgnore]
		public virtual Category Category { get; set; } = null!;
	}
}
