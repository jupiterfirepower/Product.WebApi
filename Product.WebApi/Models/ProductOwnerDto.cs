using System.ComponentModel.DataAnnotations;

namespace Product.WebApi.Models
{
    public class ProductOwnerDto
    {
        public int OwnerId { get; set; }
        [Required]
        [StringLength(150)]
        public string OwnerName { get; set; }
        [Required]
        [StringLength(100)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public string Email { get; set; }
        [StringLength(50)]
        public string Phone { get; set; }
    }
}
