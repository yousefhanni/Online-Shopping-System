using MyShop.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyShop.Domain.ViewModels
{
    public class ShoppingCartViewModel
    {
        public Product Product { get; set; }

        [Range(1, 100, ErrorMessage = "You must enter a value between 1 to 100")]
        public int Count { get; set; }

        // Add this property to hold related products
        public List<Product> RelatedProducts { get; set; }
    }
}
