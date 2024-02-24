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



        public ProductController(IProductService productServices, IWebHostEnvironment webHostEnvironment)
        {
            _productServices = productServices;

        }

        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetProducts()
        {
            try
            {
                var products = await _productServices.GetProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("paginated-product")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> PaginatedProduct([FromQuery] int pageNumber = 1, [FromQuery] int PageSize = 10)
        {
            try
            {
                return Ok(await _productServices.ProductPagination(pageNumber, PageSize));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("{id}", Name = "getproducts")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetProdectById(int id)
        {
            try
            {
                var products = await _productServices.GetProductById(id);
                return Ok(products);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("product-by-category")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetProductByCategory(int categoryId)
        {
            try
            {
                return Ok(await _productServices.GetProductByCategory(categoryId));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("clothes-by-gender")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetClothesByGender(char gender)
        {
            try
            {
                var res = await _productServices.GetClothesByGender(gender);
                return Ok(res);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }


        [HttpGet("product-by-category-name")]
        [Authorize]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> GetProductsByCategoryName(string name)
        {
            try
            {
                return Ok(await _productServices.GetProductByCategoryName(name));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddProduct([FromForm] CreateProductDTO productDto, IFormFile image)
        {
            try
            {
                var res = await _productServices.CreateProduct(productDto, image);
                return res ? Ok("Product created sucessfully !") : StatusCode(500,"Error while craeting a new product !");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                bool res = await _productServices.DeleteProduct(id);
                return res ? Ok() : StatusCode(500, "An error ocuured while deleting product !");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] CreateProductDTO productDto, IFormFile image)
        {
            try
            {
                bool res = await _productServices.UpdateProduct(id, productDto, image);
                return res ? Ok() : StatusCode(500, "An error ocuured while updating product !");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
