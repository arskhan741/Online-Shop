using Online_Shop.DTOs.CategoryDTOs;
using Online_Shop.Models;

namespace Online_Shop.Contracts
{
    public interface ICategoryRepository
    {
        Task<Category?> CreateAsync(CreateCategoryDTO createCategoryDTO);

        Task<Category?> UpdateAsync(UpdateCategoryDTO updateCategoryDTO);

        Task DeleteAsync(DeleteCategoryDTO deleteCategoryDTO);

        Task<GetCategoryDetailsDto> GetAsync(int categoryId);

        Task<IEnumerable<GetCategoryDetailsDto>> GetAllAsync();

    }
}
