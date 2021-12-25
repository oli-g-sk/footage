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

    public class EntityDao : IEntityDao
    {
        private readonly VideoContext dbContext;
        
        public EntityDao()
        {
            dbContext = new VideoContext();
        }
        
        public async Task<bool> Contains<T>(Expression<Func<T, bool>> predicate) where T : Entity 
        {
            var entities = dbContext.Set<T>();
            return await entities.AnyAsync(predicate);
        }

        public async Task Insert<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
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

        public async Task InsertRange<T>(IEnumerable<T> items) where T : Entity
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

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

        public async Task Remove<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

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

        public IEnumerable<T> Query<T>(Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            var entities = dbContext.Set<T>().AsQueryable();

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }
            
            return entities.AsEnumerable().ToList();
        }
        
        public void Dispose()
        {
            dbContext.Dispose();
        }

        private void ProcessException(Exception ex)
        {
            Debugger.Break();
            
            // TODO KURVA this thrown exception is swallowed, nothing happens :(
            throw new DbException(ex);
        }
    }
}
