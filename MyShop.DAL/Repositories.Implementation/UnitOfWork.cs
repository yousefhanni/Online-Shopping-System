using Myshop.DAL.Data;
using myshop.DataAccess.Implementation;
using MyShop.Domain.Repositories.Contract;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public ICategoryRepository Category { get; private set; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Category = new CategoryRepository(context);
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
