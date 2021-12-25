namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
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

        public async Task Insert(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await using var dbContext = new VideoContext();

            try
            {
                dbContext.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public async Task InsertRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            await using var dbContext = new VideoContext();

            try
            {
                dbContext.AddRange(items);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public async Task Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            await using var dbContext = new VideoContext();

            try
            {
                dbContext.Remove(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>>? predicate = null)
        {
            using var dbContext = new VideoContext();
            
            var entities = GetEntities(dbContext);

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }
            
            return entities.AsEnumerable().ToList();
        }

        protected abstract IQueryable<T> GetEntities(VideoContext context);

        private void ProcessException(Exception ex)
        {
            Debugger.Break();
            
            // TODO KURVA this thrown exception is swallowed, nothing happens :(
            throw new DbException(ex);
        }
    }
}