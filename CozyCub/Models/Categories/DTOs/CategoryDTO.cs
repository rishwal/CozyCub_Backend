using CozyCub.Models.ProductModels.DTOs;
using System.ComponentModel.DataAnnotations;

namespace CozyCub.Models.Categories.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
      

    }
}
