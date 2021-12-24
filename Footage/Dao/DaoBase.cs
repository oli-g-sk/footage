namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Context;
    using Footage.Model;

    public abstract class DaoBase<T> : IEntityDao<T> where T : Entity
    {
        protected readonly VideoContext DbContext;

        protected DaoBase(VideoContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Insert(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            DbContext.Add(item);
            DbContext.SaveChanges();
        }

        public void Remove(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }
            
            DbContext.Remove(item);
            DbContext.SaveChanges();
        }

        protected abstract IEnumerable<T>? Entities { get; }

        public IQueryable<T> Query()
        {
            return (Entities ?? Array.Empty<T>()).AsQueryable();
        }
    }
}