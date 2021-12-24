namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Context;
    using Footage.Model;

    public abstract class DaoBase<T> : IEntityDao<T> where T : Entity
    {
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