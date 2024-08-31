using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyShop.Domain.Models
{
    // Represents the details of individual products within an order.
    public class OrderDetails
    {
        // Unique identifier for the order details entry
        public int Id { get; set; }

        // Foreign key referencing the OrderHeader this detail belongs to
        public int OrderHeaderId { get; set; }

        // Navigation property to the OrderHeader entity
        // Represents the overall order to which these details belong
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }

        // Foreign key referencing the Product entity
        public int ProductId { get; set; }

        // Navigation property to the Product entity
        // Represents the product being ordered
        [ValidateNever]
        public Product Product { get; set; }

        // The price of the product at the time of the order
        public decimal Price { get; set; }

        // The quantity of the product being ordered
        public int Count { get; set; }
    }
}
