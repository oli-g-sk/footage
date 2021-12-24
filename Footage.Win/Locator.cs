namespace Footage.Win
{
    using Footage.Context;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel;
    using SimpleInjector;

    public static class Locator
    {
        // TODO move this class to the "core" project?
        
        private static readonly Container Container;

        static Locator()
        {
            Container = new Container();

            // DAO implementations
            Container.Register<IMediaSourceDao, MediaSourceDao>();
            Container.Register<IVideoDao, VideoDao>();
            
            // services
            Container.Register<SourcesRepository>();

            // ViewModels
            Container.Register<MainWindowViewModel>();
            Container.Register<MediaSourcesViewModel>();
            
            Container.Verify();
        }

        public static T GetInstance<T>() where T : class => Container.GetInstance<T>();
    }
}