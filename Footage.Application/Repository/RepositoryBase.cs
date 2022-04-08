using Footage.Persistence;

namespace Footage.Application.Repository
{
    public abstract class RepositoryBase
    {
        protected IEntityDao GetDao() => Locator.Create<IEntityDao>();
    }
}