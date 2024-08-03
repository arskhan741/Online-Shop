using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Contracts;
using Online_Shop.DTOs.CategoryDTOs;
using Online_Shop.Models;

namespace Online_Shop.Repository
{
    public class CategoryReposityory : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryReposityory(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Category?> CreateAsync(CreateCategoryDTO createCategoryDTO)
        {
            Category category = _mapper.Map<Category>(createCategoryDTO);

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task DeleteAsync(DeleteCategoryDTO deleteCategoryDTO)//
        {
            Category? category = null;

            category = await _context.Categories.FindAsync(deleteCategoryDTO.CategoryId);
            category ??= await _context.Categories.FindAsync(deleteCategoryDTO.CategoryName);

            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            else
            {
                throw new NullReferenceException($"category not found");
            }
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category != null)
                return category;
            else
                throw new NullReferenceException($"category not found");
        }

        public async Task<Category?> UpdateAsync(UpdateCategoryDTO updateCategoryDTO)
        {
            var category = await _context.Categories.FindAsync(updateCategoryDTO.CategoryID);

            if (category is null)
                throw new NullReferenceException("category not found");

            category.Name = updateCategoryDTO.NewName;
            category.Description = updateCategoryDTO.NewDescription;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }
    }
}
