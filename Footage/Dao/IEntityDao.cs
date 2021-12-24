namespace Footage.Dao
{
    using System.Linq;
    using Footage.Model;

    public interface IEntityDao<T> where T : Entity
    {
        void Insert(T item);

        IQueryable<T> Query();
    }
}