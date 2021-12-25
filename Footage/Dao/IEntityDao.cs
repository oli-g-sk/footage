namespace Footage.Dao
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Footage.Model;

    public interface IEntityDao : IDisposable
    {
        Task<bool> Contains<T>(Expression<Func<T, bool>> predicate) where T : Entity;
        
        Task Insert<T>(T item) where T : Entity;

        Task InsertRange<T>(IEnumerable<T> items) where T : Entity;

        Task Remove<T>(T item) where T : Entity;

        IEnumerable<T> Query<T>(Expression<Func<T, bool>>? predicate = null) where T : Entity;
        
        // TODO add a Query overload with parameters
    }
}