namespace Footage.Dao
{
    using Footage.Model;

    public interface IEntityDao<T> where T : Entity
    {
        void Insert(T item);
    }
}