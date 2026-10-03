using System.ComponentModel.DataAnnotations;

namespace eStoreASP.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }
    }
}
