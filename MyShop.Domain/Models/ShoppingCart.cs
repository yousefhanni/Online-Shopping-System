using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyShop.Domain.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }

        [NotMapped] // This property is not mapped to the database
        [ValidateNever]
        public List<Product> RelatedProducts { get; set; } = new List<Product>();
    }
}
