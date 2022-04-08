namespace Footage.Application
{
    using System;
    using Footage.Application.Service;
    using Footage.Engine;
    using GalaSoft.MvvmLight.Ioc;

    internal class Locator
    {
        private static bool initialized;

        private static IProvider Provider;

        internal static IDispatcher Dispatcher => Provider.Dispatcher;

        internal static void Initialize(IProvider provider)
        {
            Provider = provider;
            initialized = true;
        }

        internal static T Get<T>()
        {
            if (!initialized)
                throw new InvalidOperationException("Footage.Core.Initialize muse be called before using!");

            if (typeof(T) == typeof(IMediaPlayer))
                throw new ArgumentException("IMediaPlayerService cannot be accessed as singleton. Use Create<T> instead.");

            if (typeof(T) == typeof(IDialogService))
                return (T) Provider.DialogService;

            if (typeof(T) == typeof(IThumbnailMaker))
                return (T) Provider.ThumbnailMaker;
            
            return SimpleIoc.Default.GetInstance<T>();
        }

        internal static T Create<T>()
        {
            if (!initialized)
                throw new InvalidOperationException("Footage.Core.Initialize muse be called before using!");

            if (typeof(T) == typeof(IMediaPlayer))
                return (T)Provider.CreateMediaPlayer();

            return SimpleIoc.Default.GetInstanceWithoutCaching<T>();
        }
    }
}
