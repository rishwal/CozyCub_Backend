using CozyCub.Models.Categories.DTOs;
using CozyCub.Services.Category_services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CozyCub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryServices;

        // Constructor to initialize dependencies
        public CategoryController(ICategoryService categoryServices)
        {
            _categoryServices = categoryServices;
        }

        // Get all categories
        [HttpGet]
        [Authorize] // Requires authentication
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<ActionResult> GetCategories()
        {
            try
            {
                // Get all categories
                List<CategoryDTO> res = await _categoryServices.GetAllCategories();
                return res == null ? NotFound("Sorry no categories found") : Ok(res);
            }
            catch (Exception ex)
            {
                // Return server error if an exception occurs
                return StatusCode(500, ex.Message);
            }
        }

        // Get category by ID
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(object), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                // Get category by ID
                CategoryDTO res = await _categoryServices.GetCategoryById(id);

                return res != null ? Ok(res) : NotFound($"Sorry no category found with id:{id}");
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Add a new category
        [HttpPost]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(typeof(bool), 200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> AddCategory([FromBody] CategoryCreateDTO categoryDto)
        {
            try
            {
                // Add new category
                var isok = await _categoryServices.CreateCategory(categoryDto);
                return Ok(isok);
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Update category
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDTO categoryDto)
        {
            try
            {
                // Update category
                await _categoryServices.UpdateCategory(id, categoryDto);
                return Ok();
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }

        // Delete category
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")] // Requires admin role
        [ProducesResponseType(200)] // Successful response
        [ProducesResponseType(401)] // Unauthorized response
        [ProducesResponseType(500)] // Server error response
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                // Delete category
                await _categoryServices.DeleteCategory(id);
                return Ok();
            }
            catch (Exception e)
            {
                // Return server error if an exception occurs
                return StatusCode(500, e.Message);
            }
        }
    }
}
