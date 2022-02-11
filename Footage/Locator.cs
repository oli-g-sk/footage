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
    internal class Locator
    {
        internal static void RegisterDefaultDatabase()
        {
            SimpleIoc.Default.Register<IEntityDao, EntityDao>();
        }

        internal static void RegisterDefaultEngine()
        {
            SimpleIoc.Default.Register<IMediaPlayerService, MediaPlayerService>();
        }

        internal static void RegisterDefaultRepositories()
        {
            SimpleIoc.Default.Register<BookmarksRepository>();
            SimpleIoc.Default.Register<SourcesRepository>();
            SimpleIoc.Default.Register<VideoBrowserRepository>();
            SimpleIoc.Default.Register<VideoDetailRepository>();
        }

        internal static void RegisterDefaultServices()
        {
            SimpleIoc.Default.Register<ISourceScopedServiceFactory, SourceScopedServiceFactory>();
        }

        internal static T Get<T>()
        {
            return SimpleIoc.Default.GetInstance<T>();
        }

        internal static T Create<T>()
        {
            return SimpleIoc.Default.GetInstanceWithoutCaching<T>();
        }
    }
}
