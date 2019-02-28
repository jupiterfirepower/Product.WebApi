namespace Product.WebApi.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public ProductOwnerDto Owner { get; set; }
        
        public ProducerDto Producer { get; set; }

        public CategoryDto Category { get; set; }

       // [Required]
        //public int CategoryId { get; set; }

        //[Required]
        //public int OwnerId { get; set; }

        //[Required]
        ///public int ManufacturerId { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
