using Myshop.DAL.Data;
using MyShop.Domain.Repositories.Contract;
using MyShop.DAL.Repositories.Implementation;

namespace MyShop.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public IShoppingCartRepository _ShoppingCartRepository { get; private set; }
    public IOrderHeaderRepository OrderHeader { get; private set; }
    public IOrderDetailRepository OrderDetail { get; private set; }
     public IApplicationUserRepository ApplicationUser { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Category = new CategoryRepository(context);
        Product = new ProductRepository(context);
        _ShoppingCartRepository = new ShoppingCartRepository(context);
        OrderHeader = new OrderHeaderRepository(context);
        OrderDetail = new OrderDetailRepository(context);
        ApplicationUser = new ApplicationUserRepository(context);

    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
}
