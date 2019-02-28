using System.ComponentModel.DataAnnotations;

namespace Product.WebApi.Models
{
    public class ProducerDto
    {
        public int ProducerId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProducerName { get; set; }
        [Required]
        [StringLength(100)]
        public string ProducerAddress { get; set; }
    }
}
