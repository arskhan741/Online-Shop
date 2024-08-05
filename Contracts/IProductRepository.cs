using Online_Shop.DTOs.ProductDTOs;
using Online_Shop.Models;

namespace Online_Shop.Contracts
{
    public interface IProductRepository
    {
        Task<Product?> CreateAsync(CreateProductDTO createProductDTO);

        Task<Product?> UpdateAsync(UpdateProductDTO updateProductDTO);

        Task DeleteAsync(DeleteProductDTO deleteProductDTO);

        Task<GetProductDetailsDTO> GetAsync(int categoryId);

        Task<List<GetProductDetailsDTO>> GetAllAsync();
    }
}
