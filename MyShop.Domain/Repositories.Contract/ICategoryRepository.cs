using MyShop.Domain.Models;
using System;

namespace MyShop.Domain.Repositories.Contract
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task UpdateAsync(Category category);
    }
}
