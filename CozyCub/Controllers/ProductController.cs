using CozyCub.Models.ProductModels.DTOs;
using CozyCub.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productServices;

        // Constructor to initialize dependencies
        public ProductController(IProductService productServices, IWebHostEnvironment webHostEnvironment)
        {
            _productServices = productServices;
        }

        // Get all products
        [HttpGet]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                // Retrieve all products
                var products = await _productServices.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Get paginated products
        [HttpGet("paginated-product")]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> PaginatedProduct([FromQuery] int pageNumber = 1, [FromQuery] int PageSize = 10)
        {
            try
            {
                // Retrieve paginated products
                return Ok(await _productServices.ProductPagination(pageNumber, PageSize));
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get product by ID
        [HttpGet("{id}", Name = "getproduct-by-id")]
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetProdectById(int id)
        {
            try
            {
                // Retrieve product by ID
                var products = await _productServices.GetProductById(id);
                return Ok(products);
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get products by category
        [HttpGet("product-by-category")]
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetProductByCategory(int categoryId)
        {
            try
            {
                // Retrieve products by category
                return Ok(await _productServices.GetProductByCategory(categoryId));
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Get clothes by gender
        [HttpGet("clothes-by-gender")]
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> GetClothesByGender(char gender)
        {
            try
            {
                // Retrieve clothes by gender
                var res = await _productServices.GetClothesByGender(gender);
                return Ok(res);
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Get products by category name
        [HttpGet("product-by-category-name")]
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetProductsByCategoryName(string name)
        {
            try
            {
                // Retrieve products by category name
                return Ok(await _productServices.GetProductByCategoryName(name));
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Add a product
        [HttpPost]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDTO productDto, IFormFile image)
        {
            try
            {
                // Add product
                var res = await _productServices.CreateProduct(productDto, image);
                return res ? Ok("Product created successfully!") : StatusCode(500, "Error while creating a new product!");
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Delete a product
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                // Delete product
                bool res = await _productServices.DeleteProduct(id);
                return res ? Ok() : StatusCode(500, "An error occurred while deleting product!");
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Update a product
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDTO productDto, IFormFile image)
        {
            try
            {
                // Update product
                bool res = await _productServices.UpdateProduct(id, productDto, image);
                return res ? Ok() : StatusCode(500, "An error occurred while updating product!");
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }
    }
}
