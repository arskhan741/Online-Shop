using Online_Shop.DTOs.ProductDTOs;

namespace Online_Shop.DTOs.CategoryDTOs
{
    public class GetCategoryDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ICollection<GetProductDetailsDTO> Products { get; set; } = new List<GetProductDetailsDTO>();
    }
}
