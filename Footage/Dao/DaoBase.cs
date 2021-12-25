namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Footage.Context;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public abstract class DaoBase<T> : IEntityDao<T> where T : Entity
    {
        public async Task<bool> Contains(Expression<Func<T, bool>> predicate)
        {
            await using var dbContext = new VideoContext();
            var entities = GetEntities(dbContext);
            return await entities.AnyAsync(predicate);
        }

        public void Insert(T item)
        {
            using var dbContext = new VideoContext();
            
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            dbContext.Add(item);
            dbContext.SaveChanges();
        }

        public async Task InsertRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
#if DEBUG
            await Task.Delay(1000);
#endif
            
            await using var dbContext = new VideoContext();
            await dbContext.AddRangeAsync(items);
            await dbContext.SaveChangesAsync();
        }

        public void Remove(T item)
        {
            using var dbContext = new VideoContext();
            
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            dbContext.Remove(item);
            dbContext.SaveChanges();
        }

        public IEnumerable<T> Query()
        {
            using var dbContext = new VideoContext();
            
            var entities = GetEntities(dbContext);
            
            return entities.AsEnumerable().ToList();
        }

        protected abstract IQueryable<T> GetEntities(VideoContext context);
    }
}