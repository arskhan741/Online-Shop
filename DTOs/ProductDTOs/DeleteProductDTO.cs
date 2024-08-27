using System.ComponentModel.DataAnnotations;

namespace Online_Shop.DTOs.ProductDTOs
{
    public class DeleteProductDTO
    {
		[Required]
		public int ProductId { get; set; }
	}
}
