using Footage.Engine;
using Footage.Engine.LibVlc;

namespace Footage.UI
{
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
