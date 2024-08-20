using System.ComponentModel.DataAnnotations;

namespace MyShop.Web.Models
{
    // Represents a category for organizing products in the system
    public class Category
    {
        [Key]
        public int Id { get; set; }

        // Name of the category, a required field to ensure every category has a name
        [Required]
        public string Name { get; set; }

        // Optional description of the category, can be used to provide more details about the category
        public string Description { get; set; }

        // The timestamp of when the category was created, automatically set to the current date and time upon creation
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
