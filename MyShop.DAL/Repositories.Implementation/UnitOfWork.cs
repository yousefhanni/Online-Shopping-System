using Myshop.DAL.Data;
using myshop.DataAccess.Implementation;
using MyShop.Domain.Repositories.Contract;
using MyShop.DAL.Repositories.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICategoryRepository Category { get; private set; }
    public IProductRepository Product { get; private set; }
    public IShoppingCartRepository _ShoppingCartRepository { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Category = new CategoryRepository(context);
        Product = new ProductRepository(context);
        _ShoppingCartRepository = new ShoppingCartRepository(context);

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
