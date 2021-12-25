namespace Footage.Repository
{
    using System;
    using Footage.Dao;

    public abstract class RepositoryBase : IDisposable
    {
        protected IEntityDao Dao { get; }

        protected RepositoryBase()
        {
            Dao = new EntityDao();
        }
        
        public virtual void Dispose()
        {
            Dao.Dispose();
        }
    }
}