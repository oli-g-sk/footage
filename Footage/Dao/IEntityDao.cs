namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Footage.Model;

    public interface IEntityDao<T> where T : Entity
    {
        Task<bool> Contains(Expression<Func<T, bool>> predicate);
        
        void Insert(T item);

        Task InsertRange(IEnumerable<T> items);

        void Remove(T item);

        IEnumerable<T> Query();
        
        // TODO add a Query overload with parameters
    }
}