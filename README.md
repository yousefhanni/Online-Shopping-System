# Shopping MVC System

## Overview
This project is a Shopping MVC system developed using **.NET 8.0**. It is designed to offer a comprehensive solution for managing an online shopping platform. The system includes features such as product management, category management, user authentication, shopping cart functionality, order processing, and an admin dashboard for managing the entire application. The project follows a modular architecture that ensures scalability, maintainability, and ease of development.

You can check out the live demo here: [Live Demo](http://palestine-shop.runasp.net)

## Technologies Used
- **MVC .NET 8.0**
- **Repository Pattern**
- **Unit of Work**
- **N-Tier Architecture**
- **Entity Framework Core**
- **Authentication & Authorization with Microsoft.AspNetCore.Identity**
- **Sessions**
- **Pagination**
- **Payment with Stripe**
- **Admin's Dashboard**
- **Areas**
- **jQuery DataTables**
- **Toaster Notification JS**
- **Bootstrap**
- **Uploading Files**
- **Lock/Unlock User Account**
- **View Components**

## Features

### 1. Category Management
- **CRUD Operations**: Create, read, update, and delete product categories.
- **Validation**: Includes both server-side and client-side validation to ensure data integrity.
- **Security**: Anti-forgery tokens are used to protect against CSRF attacks.

### 2. Product Management
- **CRUD Operations**: Manage products with full create, read, update, and delete functionality.
- **JQuery Datatables Integration**: Enhance product listing with advanced search, sorting, and pagination features.
- **SweetAlert Integration**: Provides a user-friendly interface for confirming deletions.

### 3. User Authentication and Identity Management
- **ASP.NET Identity Integration**: Provides a robust authentication system with support for roles and claims.
- **Role-Based Access Control**: Differentiate between admin and customer roles to restrict or grant access to specific areas of the application.
- **Lock/Unlock Accounts**: Admins can lock or unlock user accounts as needed.

### 4. Shopping Cart
- **Add to Cart**: Users can add products to their shopping cart with real-time updates.
- **Cart Management**: Adjust quantities, remove items, and view total prices dynamically.
- **Session Management**: Shopping cart data is stored and managed using session state to persist user actions across different pages.

### 5. Order Processing
- **Order Creation**: Users can place orders, which are stored with detailed order headers and itemized details.
- **Payment Integration**: Supports payment processing via Stripe API.
- **Order Management**: Admins can manage and track orders, update order statuses, and handle shipping processes.

### 6. Added Orders Functionality to the Customer Area
- **Implemented `OrdersController`** to manage customer orders, allowing customers to view and manage their orders directly from the Customer area.

- **Added `MyOrders` View**:
  - This view allows customers to see a list of all their past and current orders.
  - The list includes:
    - **Order Date**: The date when the order was placed.
    - **Status**: The current status of the order (e.g., Pending, Approved, Shipped, Cancelled).
    - **Total Amount**: The total cost of the order.
    - **Actions**: Options available for each order, including the ability to view the order summary and cancel the order (if applicable).
  - Each order entry includes a "View Order Summary" button that navigates the customer to a detailed page with more information about that specific order.

- **Added `OrderSummary` View**:
  - This view provides detailed information about a specific order, including:
    - **Product Names**: Names of all products in the order.
    - **Quantities**: The quantity of each product purchased.
    - **Prices**: The price of each product.
    - **Total Cost**: The total cost of the order.
  - The summary is designed to be clear and informative, giving customers a full overview of their purchase.

- **Implemented `CancelOrder` Functionality**:
  - Available in both `CartController` and `OrdersController`.
  - Customers can cancel their orders if the order status is either "Pending" or "Approved".
  - The cancellation option is accessible directly from the `MyOrders` page, ensuring that customers can manage their orders easily.
  - The system ensures that only eligible orders can be canceled, maintaining the integrity of the order processing system.

- **Improved Styling and User Experience**:
  - Enhanced the visual design of the `MyOrders` and `OrderSummary` views to ensure a consistent and user-friendly interface.
  - The `MyOrders` page is easy to navigate, allowing customers to quickly find the information they need about their orders.
  - The `OrderSummary` page provides a detailed breakdown of the order, making it easy for customers to review their purchases.

### 7. Admin Dashboard
- **Centralized Management**: Provides an admin interface for managing categories, products, orders, and users.
- **Notifications**: Uses Toastr.js for real-time notifications of actions like creating, updating, or deleting records.
- **Reports and Analytics**: Basic analytics and reports can be generated to monitor sales and user activity.




