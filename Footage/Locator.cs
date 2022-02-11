using Footage.Dao;
using Footage.Repository;
using Footage.Service;
using GalaSoft.MvvmLight.Ioc;

namespace Footage
{
    public class Locator
    {
        public static void RegisterDefaultDatabase()
        {
            SimpleIoc.Default.Register<IEntityDao, EntityDao>();
        }

        public static void RegisterDefaultRepositories()
        {
            SimpleIoc.Default.Register<BookmarksRepository>();
            SimpleIoc.Default.Register<SourcesRepository>();
            SimpleIoc.Default.Register<VideoBrowserRepository>();
            SimpleIoc.Default.Register<VideoDetailRepository>();
        }

        public static void RegisterDefaultServices()
        {
            SimpleIoc.Default.Register<IMediaProviderFactory, MediaProviderFactory>();
        }
        
        public static void RegisterDefaultEngine()
        {
            SimpleIoc.Default.Register<IMediaPlayerService, MediaPlayerService>();
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
