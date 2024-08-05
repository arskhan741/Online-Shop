using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Contracts;
using Online_Shop.DTOs.CategoryDTOs;
using Online_Shop.Models;

namespace Online_Shop.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ApplicationDbContext context, IMapper mapper)
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

        public async Task DeleteAsync(DeleteCategoryDTO deleteCategoryDTO)
        {
            Category? category = null;

            category = await _context.Categories.FindAsync(deleteCategoryDTO.CategoryId);
            category ??= await _context.Categories.FindAsync(deleteCategoryDTO.CategoryName);

            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync(); // Ensure changes are saved
            }
            else
            {
                throw new NullReferenceException($"category not found");
            }
        }

        public async Task<IEnumerable<GetCategoryDetailsDto>> GetAllAsync()
        {
            //Use when needed, lazy loading, can cause some performance issues.

            var categories = await _context.Categories
                                           .Include(c => c.Products)
                                           .ToListAsync();

            return _mapper.Map<IEnumerable<GetCategoryDetailsDto>>(categories);
        }

        public async Task<GetCategoryDetailsDto> GetAsync(int categoryId)
        {
            var category = await _context.Categories
                                         .Include(c => c.Products)
                                         .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category != null)
                return _mapper.Map<GetCategoryDetailsDto>(category);
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
