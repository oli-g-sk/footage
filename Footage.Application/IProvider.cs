namespace Footage.Application
{
    using Footage.Application.Service;
    using Footage.Engine;

    /// <summary>
    /// Implement this interface to plug in dependencies.
    /// </summary>
    public interface IProvider
    {
        IDispatcher Dispatcher { get; }
        
        IDialogService DialogService { get; }
        
        IThumbnailMaker ThumbnailMaker { get; }

        IMediaPlayer CreateMediaPlayer();
    }
}
