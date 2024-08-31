using MyShop.Domain.Models;

namespace MyShop.Domain.Repositories.Contract
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        int IncreaseCount(ShoppingCart shoppingCart, int count);
        int DecreaseCount(ShoppingCart shoppingCart, int count);
    }
}
