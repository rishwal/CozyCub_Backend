using AutoMapper;
using CozyCub.JWT_Id;
using CozyCub.Models.ProductModels;
using CozyCub.Models.ProductModels.DTOs;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

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
        private readonly string? _hostUrl;
        private readonly IJwtService _jwtServices;

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
            _hostUrl = _configuration["HostUrl:url"];
        }




        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDTO">Product data.</param>
        /// <param name="image">Product image file.</param>
        public async Task<bool> CreateProduct(CreateProductDTO productDTO, IFormFile image)
        {
            try
            {
                string? productImage = null;


                if (image != null && image.Length > 0)
                {

                    string filleName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);

                    //Getting file Path
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", filleName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    productImage = "/Images/Products/" + filleName;




                }
                else
                {
                    productImage = "/Images/Products/placeholder.jpg";
                }


                var product = _mapper.Map<Models.ProductModels.Product>(productDTO);

                product.Image = productImage;

                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return true;


            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("Error adding product: " + ex.Message);

            }
        }


        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        public async Task<bool> DeleteProduct(int id)
        {

            try
            {
                var prd = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                _context.Products.Remove(prd);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync($"An exception occured while removing product with id {id}" + ex.Message);

                return false;
            }
        }



        /// <summary>
        /// Retrieves products by category ID.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of products within the specified category.</returns>
        public async Task<List<ProductOutputDTO>> GetProductByCategory(int categoryId)
        {
            try
            {
                var products = await _context.Products
               .Include(p => p.Category)
               .Where(p => p.CategoryId == categoryId)
               .Select(p => new ProductOutputDTO
               {
                   Id = p.Id,
                   ProductName = p.Name,
                   ProductDescription = p.Description,
                   Price = p.Price,
                   Quantity = p.Quantity,   
                   Category = p.Category.Name,
                   Gender = p.Gender,
                   ProductImage = _hostUrl + p.Image,

               }).ToListAsync();

                if (products != null)
                {
                    return products;
                }

                return [];
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"An exception occured while retrieving products with category id:{categoryId}" + ex.Message);
                return [];
            }
        }

        public async Task<List<ProductOutputDTO>> GetProductByCategoryName(string category)
        {
            try
            {
                var products = await _context.Products.
                    Include(p => p.Category)
                    .Where(c => c.Category.Name == category)
                    .Select(p => new ProductOutputDTO
                    {
                        Id = p.Id,
                        ProductName = p.Name,
                        ProductDescription = p.Description,
                        OfferPrice = p.OfferPrice,
                        Price = p.Price,
                        Quantity = p.Quantity,  
                        Gender = p.Gender,
                        Category = p.Category.Name,
                        ProductImage = _hostUrl + p.Image,

                    }).ToListAsync();

                if (products != null)
                    return products;

                return new List<ProductOutputDTO>();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("An exception occured while fetchig products with category name " + ex.Message);
                throw;
            }

        }


        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        public async Task<ProductOutputDTO> GetProductById(int id)
        {
            var prdt = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (prdt != null)
            {
                ProductOutputDTO product = new()
                {

                    Id = prdt.Id,
                    ProductName = prdt.Name,
                    ProductDescription = prdt.Description,
                    Price = prdt.Price,
                    OfferPrice = prdt.OfferPrice,
                    Gender = prdt.Gender,
                    Quantity = prdt.Quantity,
                    Category = prdt.Category.Name,
                    ProductImage = _hostUrl + prdt.Image

                };

                return product;

            }

            return new ProductOutputDTO();


        }

        ///<summary>
        ///The function is for Fetching product by gender  
        /// </summary>
        public async Task<List<ProductOutputDTO>> GetClothesByGender(char gender)
        {
            try
            {
                var clothesByGender = await _context.Products
                                      .Include(p => p.Category)
                                      .Where(p => p.CategoryId == 1 && p.Gender == gender)
                                      .Select(p => new ProductOutputDTO
                                      {
                                          Id = p.Id,
                                          ProductName = p.Name,
                                          ProductDescription = p.Description,
                                          Price = p.Price,
                                          Gender = p.Gender,
                                          Quantity = p.Quantity,
                                          Category = p.Category.Name,
                                          ProductImage = p.Image

                                      }).ToListAsync();



                if (clothesByGender.Count > 0)
                    return clothesByGender;
                return [];


            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("A");
                throw;
            }
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
                        OfferPrice = p.OfferPrice,
                        Gender = p.Gender,
                        Quantity = p.Quantity,
                        Category = p.Category.Name,
                        ProductImage = _hostUrl + p.Image

                    }).ToList();
                    return productWithCategory;
                }
                return [];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"An Exception has been occuured while fetching clothes with by gender! {ex.Message}");
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
                    Gender = p.Gender,
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
        public async Task<bool> UpdateProduct(int id, CreateProductDTO productDTO, IFormFile image)
        {
            try
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product != null)
                {
                    product.Name = productDTO.Name;
                    product.Description = productDTO.Description;
                    product.Price = productDTO.Price;
                    product.OfferPrice = productDTO.OfferPrice;
                    product.Gender = productDTO.Gender;
                    product.CategoryId = productDTO.CategoryId;


                    if (image != null && image.Length > 0)
                    {

                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Products", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }


                        product.Image = "/Images/Products/" + fileName;
                    }
                    else
                    {
                        product.Image = "Images/Products/PlaceholderImg/placeholder.jpg";
                    }


                    await _context.SaveChangesAsync();

                    return true;
                }
                else
                {
                    return false;
                    throw new InvalidOperationException($"Product with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception($"Error updating product with ID {id}: {ex.Message}", ex);
            }
        }


    }
}
