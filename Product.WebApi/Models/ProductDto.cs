using System.ComponentModel.DataAnnotations;

namespace Product.WebApi.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

        public ProductOwnerDto Owner { get; set; }
        
        public ProducerDto Producer { get; set; }

        public CategoryDto Category { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
