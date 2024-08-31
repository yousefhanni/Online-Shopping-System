
using MyShop.Domain.Repositories.Contract;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    IShoppingCartRepository _ShoppingCartRepository { get; }
    IOrderHeaderRepository OrderHeader { get; }
    IOrderDetailRepository OrderDetail { get; }
    IApplicationUserRepository ApplicationUser { get; }

    Task<int> CompleteAsync();
}
