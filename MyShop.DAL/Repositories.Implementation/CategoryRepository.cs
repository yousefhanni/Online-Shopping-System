using MyShop.Domain.Models;
using MyShop.Domain.Repositories.Contract;
using Myshop.DAL.Data;
using MyShop.DataAccess.Implementation;

namespace myshop.DataAccess.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Category category)
        {
            var categoryInDb = await _context.Categories.FindAsync(category.Id);
            if (categoryInDb != null)
            {
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                categoryInDb.CreatedTime = DateTime.Now;
            }
        }
    }
}
