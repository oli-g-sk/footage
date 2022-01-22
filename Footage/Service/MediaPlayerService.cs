namespace Footage.Service
{
    using System;
    using System.Threading.Tasks;
    using LibVLCSharp.Shared;
    using LogLevel = LibVLCSharp.Shared.LogLevel;
    using NLogLevel = NLog.LogLevel;
    using NLog;

    public sealed class MediaPlayerService : IMediaPlayerService, IDisposable
    {
        private static ILogger VlcLog => LogManager.GetLogger("LibVLC");

        private static readonly LibVLC LibVlc;

        private readonly MediaPlayer helperPlayer;

        public long Duration { get; private set; }
        
        public MediaPlayer Player { get; }

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
            Player = new MediaPlayer(LibVlc);
            helperPlayer = new MediaPlayer(LibVlc);
        }

        public async Task LoadMedia(string uri)
        {
            await UnloadMedia();
            Player.Media = new Media(LibVlc, new Uri(uri));
            await Player.Media.Parse();
            Duration = Player.Media.Duration;
            await Task.CompletedTask;
        }

        public async Task UnloadMedia()
        {
            Player.Media?.Dispose();
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
            helperPlayer.Dispose();
            Player.Dispose();
        }
    }
}