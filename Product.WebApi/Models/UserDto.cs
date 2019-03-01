using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.WebApi.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UserDto
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(6)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        [MinLength(8)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public string Email { get; set; }

        [StringLength(150)]
        public string FirstName { get; set; }

        [StringLength(150)]
        public string LastName { get; set; }
    }
}
