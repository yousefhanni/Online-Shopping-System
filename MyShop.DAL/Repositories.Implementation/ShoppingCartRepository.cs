    using Myshop.DAL.Data;
    using MyShop.Domain.Models;
    using MyShop.Domain.Repositories.Contract;

    namespace MyShop.DAL.Repositories.Implementation
    {
        public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
        {
            private readonly ApplicationDbContext _context;

            public ShoppingCartRepository(ApplicationDbContext context) : base(context)
            {
                _context = context;
            }

            public int DecreaseCount(ShoppingCart shoppingCart, int count)
            {
                shoppingCart.Count -= count;
                return shoppingCart.Count;
            }

            public int IncreaseCount(ShoppingCart shoppingCart, int count)
            {
                shoppingCart.Count += count;
                return shoppingCart.Count;
            }
        }
    }
