using MyShop.Domain.Models;

namespace MyShop.Domain.Repositories.Contract
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task UpdateAsync(Product product);
    }
}
    