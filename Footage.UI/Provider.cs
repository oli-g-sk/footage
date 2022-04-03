using Footage.Engine;

namespace Footage.UI
{
    using System;
    using Footage.Engine.MediaPlayer.LibVlc;
    using Footage.Engine.ThumbnailMaker.FFmpeg;
    using Footage.Service;
    using Footage.UI.Services;

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
