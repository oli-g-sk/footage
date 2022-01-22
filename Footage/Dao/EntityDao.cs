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
    using NLog;

    public class EntityDao : IEntityDao
    {
        private readonly VideoContext dbContext;
        
        public EntityDao()
        {
            dbContext = new VideoContext();
        }

        public async Task Commit()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> Contains<T>(Expression<Func<T, bool>> predicate) where T : Entity 
        {
            var entities = dbContext.Set<T>();
            return await entities.AnyAsync(predicate);
        }

        public Task Insert<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            try
            {
                dbContext.Add(item);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task InsertRange<T>(IEnumerable<T> items) where T : Entity
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            try
            {
                dbContext.AddRange(items);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }

            return Task.CompletedTask;
        }

        public Task Remove<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            try
            {
                dbContext.Remove(item);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task Update<T>(T item) where T : Entity
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            try
            {
                dbContext.Update(item);
                item.NotifyEntryUpdated();
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public Task UpdateRange<T>(IEnumerable<T> items) where T : Entity
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }
            
            try
            {
                dbContext.UpdateRange(items);
                foreach (var item in items)
                {
                    item.NotifyEntryUpdated();
                }
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
            
            return Task.CompletedTask;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>>? predicate = null) where T : Entity
        {
            var entities = dbContext.Set<T>().AsQueryable();

            if (predicate != null)
            {
                entities = entities.Where(predicate);
            }
            
            return entities;
        }
        
        public void Dispose()
        {
            dbContext.Dispose();
        }

        private void ProcessException(Exception ex)
        {
            Debugger.Break();
            
            // TODO KURVA this thrown exception is swallowed, nothing happens :(
            Logger.Log(LogLevel.Error, $"DB exception occurred: {ex}");
            throw new DbException(ex);
        }

        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    }
}
