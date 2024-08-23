using MyShop.Domain.Repositories.Contract;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Category { get; }

    Task<int> CompleteAsync();
}
