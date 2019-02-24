namespace Product.WebApi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductOwner")]
    public class ProductOwner
    {
        [Key]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(150)]
        public string OwnerName { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "OwnerAddress")]
        public string Address { get; set; }
        [StringLength(50)]
        [Column(TypeName = "OwnerEmail")]
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public string Email { get; set; }
        [StringLength(50)]
        [Column(TypeName = "OwnerPhone")]
        public string Phone { get; set; }
    }

    public class Manufacturer
    {
        [Key]
        public int ManufacturerId { get; set; }

        [Required]
        [StringLength(150)]
        public string ManufactureName { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "ManufactureAddress")]
        public string Address { get; set; }
    }

    [Table("Products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "ProductName")]
        public string ProductName { get; set; }

        [StringLength(250)]
        [Column(TypeName = "ProductDescription")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "ProductPrice")]
        public decimal Price { get; set; }

        [ForeignKey("OwnerId")]
        public virtual ProductOwner Owner { get; set; }
        [ForeignKey("ManufacturerId")]
        public virtual Manufacturer Producer { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int ManufacturerId { get; set; }
    }

    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        [Column(TypeName = "UserName")]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "UserPassword")]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [Column(TypeName = "UserEmail")]
        [EmailAddress(ErrorMessage = "The email format is not valid")]
        public string Email { get; set; }

        [StringLength(150)]
        [Column(TypeName = "FirstName")]
        public string FirstName { get; set; }

        [StringLength(150)]
        [Column(TypeName = "LastName")]
        public string LastName { get; set; }
    }

    [Table("Categories")]
    public class Category 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [DataType(DataType.Text), MaxLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Text), MaxLength(150)]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }
    }
}
