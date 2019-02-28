using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Product.WebApi.Models
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public CategoryDto Parent { get; set; }

        public ICollection<CategoryDto> Children { get; set; }
    }
}
