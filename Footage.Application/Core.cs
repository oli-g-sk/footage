using Footage.Persistence.EFCore.Dao;

namespace Footage.Application
{
    using Footage.Application.Repository;
    using Footage.Application.Service;
    using Footage.Persistence;
    using GalaSoft.MvvmLight.Ioc;

    public class Core
    {
        public static void Initialize(IProvider implProvider)
        {
            RegisterDefaultDatabase();
            RegisterDefaultRepositories();
            RegisterDefaultServices();

            Locator.Initialize(implProvider);
        }

        internal static void RegisterDefaultDatabase()
        {
            SimpleIoc.Default.Register<IEntityDao, EntityDao>();
        }

        internal static void RegisterDefaultRepositories()
        {
            SimpleIoc.Default.Register<BookmarksRepository>();
            SimpleIoc.Default.Register<ProjectsRepository>();
            SimpleIoc.Default.Register<SourcesRepository>();
            SimpleIoc.Default.Register<LibraryRepository>();
            SimpleIoc.Default.Register<VideoBrowserRepository>();
            SimpleIoc.Default.Register<VideoDetailRepository>();
        }

        internal static void RegisterDefaultServices()
        {
            SimpleIoc.Default.Register<ISourceScopedServiceFactory, SourceScopedServiceFactory>();
            SimpleIoc.Default.Register<IPersistenceService, PersistenceService>();
            SimpleIoc.Default.Register<IThumbnailManager, ThumbnailManager>();
        }
    }
}
