namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Dao;

    public abstract class RepositoryBase : IDisposable
    {
        protected EntityDao dao { get; private set; }

        protected RepositoryBase()
        {
            dao = new EntityDao();
        }
        
        public void Dispose()
        {
            dao.Dispose();
        }
    }
}