using AutoMapper;
using CozyCub.Models.Categories.DTOs;
using CozyCub.Models.Classification;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Internal;

namespace CozyCub.Services.Category_services
{
    /// <summary>
    /// Service class for managing categories.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public CategoryService(ApplicationDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="createDTO">The DTO containing information about the category to create.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>
        public async Task<bool> CreateCategory(CategoryCreateDTO createDTO)
        {
            try
            {
                var newCategory = _mapper.Map<Category>(createDTO);
                await _context.Categories.AddAsync(newCategory);

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="category">The DTO containing updated information about the category.</param>
        /// <returns>A boolean indicating whether the operation was successful.</returns>

        public async Task<bool> UpdateCategory(int id, CategoryCreateDTO category)
        {
            try
            {
                Category? UpdateCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

                if (UpdateCategory != null)
                {
                    UpdateCategory.Name = category.Name;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Deletes a category from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>The deleted category DTO if successful; otherwise, null.</returns>
        /// <remarks>
        /// This method retrieves the category with the specified ID from the database,
        /// removes it from the context, and saves changes. If the category is not found,
        /// null is returned. Any exceptions that occur during the process are logged, 
        /// and null is returned as well.
        /// </remarks>
        public async Task<CategoryDTO> DeleteCategory(int id)
        {
            try
            {
                Category? categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);


                if (categoryToDelete != null)
                {
                    _context.Categories.Remove(categoryToDelete);
                    await _context.SaveChangesAsync();

                    return _mapper.Map<CategoryDTO>(categoryToDelete);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return null;

            }
        }


        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        /// <returns>A list of category DTOs.</returns>
        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            try
            {
                var AllCategories = await _context.Categories.ToListAsync();
                List<CategoryDTO> categories = _mapper.Map<List<CategoryDTO>>(AllCategories);
                return categories ?? null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving category: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Retrieves a category by its ID from the database.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The category DTO with the specified ID.</returns>
        public async Task<CategoryDTO> GetCategoryById(int id)
        {
            try
            {
                Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
                return _mapper.Map<CategoryDTO>(category) ?? null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retreiving category: {ex.Message}");
                return null;
            }
        }


    }
}
