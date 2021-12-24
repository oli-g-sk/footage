namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Context;

    public abstract class DaoBase<T>
    {
        protected VideoContext DbContext;
        
        public DaoBase(VideoContext dbContext)
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
        }

        protected abstract IEnumerable<T> Entities { get; }

        public IQueryable<T> Query()
        {
            return Entities.AsQueryable();
        }
    }
}