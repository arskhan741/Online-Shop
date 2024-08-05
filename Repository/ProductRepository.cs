using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Online_Shop.Contracts;
using Online_Shop.DTOs.ProductDTOs;
using Online_Shop.Models;

namespace Online_Shop.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product?> CreateAsync(CreateProductDTO createProductDTO)
        {
            Product product = _mapper.Map<Product>(createProductDTO);

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(DeleteProductDTO deleteProductDTO)
        {
            Product? product = null;

            product = await _context.Products.FindAsync(deleteProductDTO.ProductId);

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
            List<Product> products = await _context.Products.Include(p => p.Category).ToListAsync();

            List<GetProductDetailsDTO> productDetailsList = _mapper.Map<List<GetProductDetailsDTO>>(products);

            foreach (var productDetails in productDetailsList)
            {
                Category? category = await _context.Categories.FindAsync(productDetails.CategoryId);
                productDetails.CategoryName = (category is null) ? throw new NullReferenceException($"invalid category") : category.Name;
            }

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

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }
    }
}
