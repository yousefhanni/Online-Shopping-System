using MyShop.Domain.Models;
using MyShop.Domain.Repositories.Contract;
using System.Threading.Tasks;

namespace myshop.Entities.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task UpdateAsync(Product product);
    }
}
    