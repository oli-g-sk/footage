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
    public class Locator
    {
        public static void RegisterDefaultDatabase()
        {
            SimpleIoc.Default.Register<IEntityDao, EntityDao>();
        }

        public static void RegisterDefaultEngine()
        {
            SimpleIoc.Default.Register<IMediaPlayerService, MediaPlayerService>();
        }

        public static void RegisterDefaultRepositories()
        {
            SimpleIoc.Default.Register<BookmarksRepository>();
            SimpleIoc.Default.Register<SourcesRepository>();
            SimpleIoc.Default.Register<VideoBrowserRepository>();
        }

        public static void RegisterDefaultServices()
        {
            SimpleIoc.Default.Register<IMediaProviderFactory, MediaProviderFactory>();
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
