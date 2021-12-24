namespace Footage.Dao
{
    using System.Collections.Generic;
    using Footage.Model;

    public interface IEntityDao<T> where T : Entity
    {
        void Insert(T item);

        void Remove(T item);

        IEnumerable<T> Query();
        
        // TODO add a Query overload with parameters
    }
}