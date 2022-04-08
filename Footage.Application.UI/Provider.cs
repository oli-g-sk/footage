using Footage.Application.Service;
using Footage.Application.UI.Services;
using Footage.Engine;
using Footage.Engine.MediaPlayer.LibVlc;
using Footage.Engine.ThumbnailMaker.FFmpeg;

namespace Footage.Application.UI
{
    internal class Provider : IProvider
    {
        public IDispatcher Dispatcher { get; } = new AvaloniaDispatcher();

        public IDialogService DialogService { get; } = new AvaloniaDialogService();

        public IThumbnailMaker ThumbnailMaker { get; } = new ThumbnailMaker();

        public IMediaPlayer CreateMediaPlayer()
        {
            return new MediaPlayer();
        }
    }
}
