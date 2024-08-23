using System.ComponentModel.DataAnnotations;

namespace MyShop.Domain.Models
{
    // Represents a category for organizing products in the system
    public class Category
    {
        public int Id { get; set; }

        // Name of the category, a required field to ensure every category has a name
        [MaxLength(100, ErrorMessage = "The name cannot exceed 100 characters.")]
        public string Name { get; set; }

        // Optional description of the category, can be used to provide more details about the category
        [MaxLength(500, ErrorMessage = "The description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
