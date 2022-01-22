namespace Footage.Repository
{
    using System;
    using Footage.Dao;

    public abstract class RepositoryBase
    {
        protected IEntityDao GetDao() => Locator.Create<IEntityDao>();
    }
}