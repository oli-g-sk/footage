namespace Footage.Engine.MediaPlayer.LibVlc
{
    using System;
    using System.Threading.Tasks;
    using Footage.Engine;
    using LibVLCSharp.Shared;
    using NLog;
    using LogLevel = LibVLCSharp.Shared.LogLevel;
    using NLogLevel = NLog.LogLevel;

    public sealed class MediaPlayer : IMediaPlayer, IDisposable
    {
        private int volume;

        private static ILogger VlcLog => LogManager.GetLogger("LibVLC");
        
        private static ILogger Log => LogManager.GetCurrentClassLogger();

        private static readonly LibVLC LibVlc;

        public LibVLCSharp.Shared.MediaPlayer Player { get; }

        public long Duration { get; private set; }

        public float Position 
        { 
            get => Player.Position; 
            set => Player.Position = value; 
        }

        public int Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (value < 0 || value > 100) 
                { 
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                volume = value;
                Player.Volume = value;
            }
        }

        public bool IsPlaying => Player.IsPlaying;

        public bool IsMediaLoaded => Player.Media != null;

        public event EventHandler PositionChanged;

        static MediaPlayer()
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

        public MediaPlayer()
        {
            Player = new LibVLCSharp.Shared.MediaPlayer(LibVlc);
            Player.PositionChanged += Player_PositionChanged;
        }

        public async Task Play()
        {
            Player.Play();
            Player.Volume = volume;
            await Task.CompletedTask;
        }

        public async Task Pause()
        {
            Player.Pause();
            await Task.CompletedTask;
        }

        public async Task Stop()
        {
            Player.Stop();
            await Task.CompletedTask;
        }

        public async Task LoadMedia(string uri)
        {
            await UnloadMedia(); // TODO LATER is unload needed before load?
            Player.Media = new Media(LibVlc, new Uri(uri));
            await Player.Media.Parse();
            Duration = Player.Media.Duration;
            Log.Debug($"Loaded media file: {uri}.");
            await Task.CompletedTask;
        }

        public async Task UnloadMedia()
        {
            Player.Media?.Dispose();
            Player.Media = null;
            Duration = 0;
            Log.Debug("Unloaded media.");
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
            Player.Dispose();
        }

        private void Player_PositionChanged(object? sender, MediaPlayerPositionChangedEventArgs e)
        {
            PositionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}