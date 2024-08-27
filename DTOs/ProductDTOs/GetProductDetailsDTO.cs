namespace Online_Shop.DTOs.ProductDTOs
{
    public class GetProductDetailsDTO
    {
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = string.Empty;

		public decimal Price { get; set; }
		public int Stock { get; set; }
		public bool IsFeatured { get; set; }
		public float Rating { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime UpdatedOn { get; set; }
	}
}
