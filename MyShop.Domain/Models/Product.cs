using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;

namespace MyShop.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        [DisplayName("Image")]
        [ValidateNever]

        public string Img { get; set; }

        public decimal Price { get; set; }

        [DisplayName("Category")]
        public int CategoryId { get; set; }  // Required
        [ValidateNever]
        public Category Category { get; set; }  // Required by EF Core

        // Add the IsFeatured property
        public bool IsFeatured { get; set; }  // New property to indicate featured products
    }
}

