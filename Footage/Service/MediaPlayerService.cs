namespace Footage.Service
{
    using System;
    using System.Threading.Tasks;
    using LibVLCSharp.Shared;

    public sealed class MediaPlayerService : IMediaPlayerService, IDisposable
    {
        private static readonly LibVLC LibVlc;

        private readonly MediaPlayer helperPlayer;
        
        public MediaPlayer Player { get; }

        static MediaPlayerService()
        {
            Core.Initialize();
            LibVlc = new LibVLC();
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
            await Task.CompletedTask;
        }

        public async Task UnloadMedia()
        {
            Player.Media?.Dispose();
            await Task.CompletedTask;
        }

        public async Task<long> GetVideoDuration(string videoUri)
        {
            helperPlayer.Media = new Media(LibVlc, new Uri(videoUri));
            await helperPlayer.Media.Parse();
            long duration = helperPlayer.Media.Duration; 
            helperPlayer.Media.Dispose();
            helperPlayer.Media = null;
            return duration;
        }

        public void Dispose()
        {
            helperPlayer.Dispose();
            Player.Dispose();
        }
    }
}