using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace MyShop.Domain.Models
{
    // Represents the main order information, including customer details, payment, and shipping info.
    public class OrderHeader
    {
        // Unique identifier for the order
        public int Id { get; set; }

        // Identifier for the user who placed the order
        public string ApplicationUserId { get; set; }

        // Navigation property for the associated user who placed the order
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        // Date and time when the order was placed
        public DateTime OrderDate { get; set; }

        // Date and time when the order is expected to be shipped
        public DateTime ShippingDate { get; set; }

        // Total price of the order, calculated by summing up the prices of individual items
        public decimal TotalPrice { get; set; }

        // Status of the order, e.g., "Pending", "Shipped", "Delivered", etc.
        public string? OrderStatus { get; set; }

        // Status of the payment, e.g., "Pending", "Completed", "Failed", etc.
        public string? PaymentStatus { get; set; }

        // Tracking number provided by the carrier for the shipped order
        public string? TrakcingNumber { get; set; } // Note: There's a typo here; should be "TrackingNumber"

        // Carrier responsible for delivering the order, e.g., "FedEx", "UPS", etc.
        public string? Carrier { get; set; }

        // Date and time when the payment was processed
        public DateTime PaymentDate { get; set; }

        // Stripe-related properties used for handling payments
        public string? SessionId { get; set; } // Stripe session ID for the payment
        public string? PaymentIntentId { get; set; } // Stripe payment intent ID

        // User information for shipping the order
        public string Name { get; set; } // Name of the recipient
        public string Address { get; set; } // Shipping address
        public string City { get; set; } // City for the shipping address
        public string? PhoneNumber { get; set; } // Contact phone number (optional)
    }
}
