namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Dao;

    public abstract class RepositoryBase : IDisposable
    {
        protected abstract IEnumerable<IDisposable> GetDisposables();
        
        public void Dispose()
        {
            foreach (var disposable in GetDisposables())
            {
                disposable.Dispose();
            }
        }
    }
}