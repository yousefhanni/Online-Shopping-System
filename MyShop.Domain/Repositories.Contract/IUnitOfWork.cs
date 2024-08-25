using myshop.Entities.Repositories;
using MyShop.Domain.Repositories.Contract;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Category { get; }
    IProductRepository Product { get; }

    Task<int> CompleteAsync();
}
