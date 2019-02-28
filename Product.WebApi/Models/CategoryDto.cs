using System.Collections.Generic;

namespace Product.WebApi.Models
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public CategoryDto Parent { get; set; }

        public ICollection<CategoryDto> Children { get; set; }
    }
}
