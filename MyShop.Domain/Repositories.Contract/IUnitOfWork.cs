
using MyShop.Domain.Repositories.Contract;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }
    IShoppingCartRepository _ShoppingCartRepository { get; }

    Task<int> CompleteAsync();
}
