using Footage.Dao;
using Footage.Repository;
using Footage.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage
{
    public class Core
    {
        public static void Initialize(IProvider implProvider)
        {
            RegisterDefaultDatabase();
            RegisterDefaultRepositories();
            RegisterDefaultServices();

            Locator.Initialize(implProvider);
            
            Locator.Get<IPersistenceService>().Initialize();
        }

        internal static void RegisterDefaultDatabase()
        {
            SimpleIoc.Default.Register<IEntityDao, EntityDao>();
        }

        internal static void RegisterDefaultRepositories()
        {
            SimpleIoc.Default.Register<BookmarksRepository>();
            SimpleIoc.Default.Register<SourcesRepository>();
            SimpleIoc.Default.Register<LibraryRepository>();
            SimpleIoc.Default.Register<VideoBrowserRepository>();
            SimpleIoc.Default.Register<VideoDetailRepository>();
        }

        internal static void RegisterDefaultServices()
        {
            SimpleIoc.Default.Register<ISourceScopedServiceFactory, SourceScopedServiceFactory>();
            SimpleIoc.Default.Register<IPersistenceService, PersistenceService>();
        }
    }
}
