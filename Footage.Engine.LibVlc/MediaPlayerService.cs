namespace Footage.Engine.LibVlc
{
    using System;
    using System.Threading.Tasks;
    using LibVLCSharp.Shared;
    using LogLevel = LibVLCSharp.Shared.LogLevel;
    using NLogLevel = NLog.LogLevel;
    using NLog;
    using Footage.Engine;

    public sealed class MediaPlayerService : IMediaPlayerService, IDisposable
    {
        private static ILogger VlcLog => LogManager.GetLogger("LibVLC");

        private static readonly LibVLC LibVlc;

        private readonly MediaPlayer player;

        public long Duration { get; private set; }

        public float Position 
        { 
            get => player.Position; 
            set => player.Position = value; 
        }

        public bool IsPlaying => player.IsPlaying;

        public bool IsMediaLoaded => player.Media != null;

        public event EventHandler PositionChanged;

        static MediaPlayerService()
        {
            Core.Initialize();
            LibVlc = new LibVLC();
            LibVlc.Log += LibVlc_Log;
        }

        private static void LibVlc_Log(object? sender, LogEventArgs e)
        {
            NLogLevel? nLogLevel = null;

            switch (e.Level)
            {
                case LogLevel.Debug:
                    nLogLevel = NLogLevel.Trace;
                    break;
                case LogLevel.Notice:
                    nLogLevel = NLogLevel.Info;
                    break;
                case LogLevel.Warning:
                    nLogLevel = NLogLevel.Warn;
                    break;
                case LogLevel.Error:
                    nLogLevel= NLogLevel.Error;
                    break;
            }

            if (nLogLevel != null)
            {
                VlcLog.Log(nLogLevel, e.Message);
            }
        }

        public MediaPlayerService()
        {
            player = new MediaPlayer(LibVlc);
            player.PositionChanged += Player_PositionChanged;
        }

        public async Task Play()
        {
            player.Play();
            await Task.CompletedTask;
        }

        public async Task Pause()
        {
            player.Pause();
            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            player.Stop();
            await Task.CompletedTask;
        }

        public async Task LoadMedia(string uri)
        {
            await UnloadMedia();
            player.Media = new Media(LibVlc, new Uri(uri));
            await player.Media.Parse();
            Duration = player.Media.Duration;
            await Task.CompletedTask;
        }

        public async Task UnloadMedia()
        {
            player.Media?.Dispose();
            Duration = 0;
            await Task.CompletedTask;
        }

        public async Task<long> GetVideoDuration(string videoUri)
        {
            using var media = new Media(LibVlc, new Uri(videoUri));
            await media.Parse();
            return media.Duration; 
        }

        public void Dispose()
        {
            player.Dispose();
        }

        private void Player_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}