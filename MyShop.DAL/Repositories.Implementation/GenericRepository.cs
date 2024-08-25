using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Myshop.DAL.Data;
using MyShop.Domain.Repositories.Contract;

namespace MyShop.DataAccess.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            //Products.Add(product);
            _dbSet.Add(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null) // if True => Condition(Where)
            {
                // query = Products.Where(x => x.Id == id)
                query = query.Where(predicate);
            }

            if (includeProperties != null)
            {
                #region Possible Scenarios
                // query = Products.Where(x => x.Id == id) 
                // if predicate is not null and includeProperties is null

                // query = Products.Where(x => x.Id == id).Include("Category").ToList(); 
                // if predicate is not null and includeProperties is not null

                // query = Products.Include("Category,Logos,Users").ToList(); 
                // if predicate is null and includeProperties is not null

                // query = Products.ToList(); 
                // if predicate is null and includeProperties is null
                #endregion

                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
            }

        public async Task<T> GetItemAsync(Expression<Func<T, bool>>? predicate = null, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return await query.SingleOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
