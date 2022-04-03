using Footage.Engine;

namespace Footage.UI
{
    using Footage.Engine.MediaPlayer.LibVlc;
    using Footage.Service;
    using Footage.UI.Services;

    internal class Provider : IProvider
    {
        public IDispatcher Dispatcher { get; } = new AvaloniaDispatcher();

        public IDialogService DialogService { get; } = new AvaloniaDialogService();

        public IMediaPlayer CreateMediaPlayer()
        {
            return new MediaPlayer();
        }
    }
}
