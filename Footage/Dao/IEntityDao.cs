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
        
        Task Insert(T item);

        Task InsertRange(IEnumerable<T> items);

        Task Remove(T item);

        IEnumerable<T> Query(Expression<Func<T, bool>>? predicate = null);
        
        // TODO add a Query overload with parameters
    }
}