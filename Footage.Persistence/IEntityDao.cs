namespace Footage.Persistence
{
    using Footage.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IEntityDao : IDisposable
    {
        Task<bool> Contains<T>(Expression<Func<T, bool>> predicate) where T : Entity;

        Task<T> Get<T>(int id) where T : Entity;

        Task Insert<T>(T item) where T : Entity;

        Task InsertRange<T>(IEnumerable<T> items) where T : Entity;

        Task Remove<T>(T item) where T : Entity;

        Task Update<T>(T item) where T : Entity;

        Task UpdateRange<T>(IEnumerable<T> items) where T : Entity;

        IQueryable<T> Query<T>(Expression<Func<T, bool>>? predicate = null) where T : Entity;

        Task Commit();
    }
}