using AutoMapper;
using CozyCub.Models.ProductModels;
using CozyCub.Models.ProductModels.DTOs;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace CozyCub.Services.ProductService
{
    /// <summary>
    /// Service class for managing products.
    /// </summary>
    public class ProductServices : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;


        /// <summary>
        /// Constructor for ProductServices.
        /// </summary>
        /// <param name="configuration">Application configuration.</param>
        /// <param name="context">Database context.</param>
        /// <param name="mapper">AutoMapper instance.</param>
        /// <param name="environment">Hosting environment.</param>
        public ProductServices(IConfiguration configuration, ApplicationDbContext context, IMapper mapper, IWebHostEnvironment environment)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _webHostEnvironment = environment ?? throw new ArgumentNullException(nameof(environment));
        }


        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDTO">Product data.</param>
        /// <param name="image">Product image file.</param>
        public async Task AddProduct(CreateProductDTO productDTO, IFormFile image)
        {
            try
            {
                string productImage = null;
               

                if (image != null && image.Length > 0)
                {
                    string filleName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);


                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", filleName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    productImage =  "/Images/Products/" + filleName;

                }
                else
                {
                    productImage = "/Images/Products/placeholder.jpg";
                }


                var product = _mapper.Map<Models.ProductModels.Product>(productDTO);

                product.Image = productImage;



                _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw new Exception("Error adding product: " + ex.Message);
                
            }
        }


        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        public async Task DeleteProduct(int id)
        {
            try
            {
                var productToDelete = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                throw;
            }
        }



        /// <summary>
        /// Retrieves products by category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of products within the specified category.</returns>
        public async Task<List<ProductOutputDTO>> GetProductByCategory(int categoryId)
        {
            var products = await _context.Products.Include(p => p.Category).Where(p => p.CategoryId == categoryId).Select(p => new ProductOutputDTO
            {
                Id = p.Id,
                ProductName = p.Name,
                ProductDescription = p.Description,
                Price = p.Price,
                Category = p.Category.Name,
                ProductImage = p.Image,

            }).ToListAsync();

            if (products != null)
            {
                return products;
            }

            return new List<ProductOutputDTO>();
        }



        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<ProductOutputDTO> GetProductById(int id)
        {
            var prd = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (prd == null)
            {
                ProductOutputDTO product = new ProductOutputDTO
                {

                    Id = prd.Id,
                    ProductName = prd.Name,
                    ProductDescription = prd.Description,
                    Price = prd.Price,
                    Category = prd.Category.Name,
                    ProductImage = prd.Image

                };

                return product;

            }

            return new ProductOutputDTO();


        }


        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of all products.</returns>
        public async Task<List<ProductOutputDTO>> GetProducts()
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category).ToListAsync();
                if (products.Count > 0)
                {
                    var productWithCategory = products.Select(p => new ProductOutputDTO
                    {
                        Id = p.Id,
                        ProductName = p.Name,
                        ProductDescription = p.Description,
                        Price = p.Price,
                        Category = p.Category.Name,
                        ProductImage = p.Image
                    }).ToList();
                    return productWithCategory;
                }
                return new List<ProductOutputDTO>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The size of each page.</param>
        /// <returns>A paginated list of products.</returns>
        public async Task<List<ProductOutputDTO>> ProductPagination(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var products = await _context.Products.Include(p => p.Category)
                                                    .Skip((pageNumber - 1) * pageSize)
                                                    .Take(pageSize)
                                                    .ToListAsync();

                var paginatedProducts = products.Select(p => new ProductOutputDTO
                {
                    Id = p.Id,
                    ProductName = p.Name,
                    ProductDescription = p.Description,
                    Price = p.Price,
                    ProductImage = p.Image,
                    Category = p.Category.Name
                }).ToList();

                return paginatedProducts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }





        /// <summary>
        /// Updates a product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDTO">The updated product data.</param>
        /// <param name="image">The product image file.</param>
        public async Task UpdateProduct(int id, CreateProductDTO productDTO, IFormFile image)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    product.Name = productDTO.Name;
                    product.Description = productDTO.Description;
                    product.Price = productDTO.Price;


                    if (image != null && image.Length > 0)
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", "Product", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }


                        product.Image = "/Uploads/Product/" + fileName;
                    }
                    else
                    {
                        product.Image = "/Uploads/common/noimage.png";
                    }


                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new InvalidOperationException($"Product with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {

                throw new Exception($"Error updating product with ID {id}: {ex.Message}", ex);
            }
        }
    }
}
