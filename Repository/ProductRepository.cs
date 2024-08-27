using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Online_Shop.Contracts;
using Online_Shop.DTOs.ProductDTOs;
using Online_Shop.Models;

namespace Online_Shop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        private readonly string _cacheKey = "productsCacheKey";

        public ProductRepository(ApplicationDbContext context, IMapper mapper, IMemoryCache cache)
        {
            _context = context;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Product?> CreateAsync(CreateProductDTO createProductDTO)
        {
            Product product = _mapper.Map<Product>(createProductDTO);

			product.CreatedOn = DateTime.UtcNow;
			product.UpdatedOn = DateTime.UtcNow;

			await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            InvalidateCache();

            return product;
        }

        public async Task DeleteAsync(DeleteProductDTO deleteProductDTO)
        {
            Product? product = null;

            product = await _context.Products.FindAsync(deleteProductDTO.ProductId);

            InvalidateCache();

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync(); // Ensure changes are saved
            }
            else
            {
                throw new NullReferenceException($"product not found");
            }
        }

        public async Task<List<GetProductDetailsDTO>> GetAllAsync()
        {
            if (_cache.TryGetValue(_cacheKey, out List<GetProductDetailsDTO>? cachedProducts))
            {
                return cachedProducts!;
            }

            List<Product> products = await _context.Products.Include(p => p.Category).ToListAsync();

            List<GetProductDetailsDTO> productDetailsList = _mapper.Map<List<GetProductDetailsDTO>>(products);

            foreach (var productDetails in productDetailsList)
            {
                Category? category = await _context.Categories.FindAsync(productDetails.CategoryId);
                productDetails.CategoryName = (category is null) ? throw new NullReferenceException($"invalid category") : category.Name;
            }

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(100), // Cache for 100 minutes
                SlidingExpiration = TimeSpan.FromMinutes(100) // Expire if not accessed for 100 minutes
            };

            _cache.Set(_cacheKey, productDetailsList, cacheOptions);

            return productDetailsList;
        }

        public async Task<GetProductDetailsDTO> GetAsync(int productId)
        {
            Product? product = await _context.Products.FindAsync(productId);

            GetProductDetailsDTO? productDetails = _mapper.Map<GetProductDetailsDTO>(product);

            Category? category = await _context.Categories.FindAsync(productDetails.CategoryId);

            productDetails.CategoryName = (category is null) ? throw new NullReferenceException($"invalid category") : category.Name;

            if (product != null)
                return productDetails;
            else
                throw new NullReferenceException($"product not found");
        }

        public async Task<Product?> UpdateAsync(UpdateProductDTO updateProductDTO)
        {
            var product = await _context.Products.FindAsync(updateProductDTO.ProductId);

            if (product is null)
                throw new NullReferenceException("product not found");

            product.Name = updateProductDTO.NewName;
            product.Description = updateProductDTO.NewDescription;
            product.CategoryId = updateProductDTO.CategoryId;
            product.UpdatedOn = DateTime.UtcNow;
            product.Price = updateProductDTO.NewPrice;
            product.Stock = updateProductDTO.NewStock;
            product.IsFeatured = updateProductDTO.IsFeatured;
            product.Rating = updateProductDTO.NewRating;

            //_mapper.Map(product, updateProductDTO);

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            InvalidateCache();

            return product;
        }

        // Invalidate cache, on update add or delete
        private void InvalidateCache() =>  _cache.Remove(_cacheKey);

    }
}
