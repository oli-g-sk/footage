namespace Footage.Service
{
    using System;
    using System.Threading.Tasks;
    using Footage.Model;
    using LibVLCSharp.Shared;

    public sealed class MediaPlayerService : IMediaPlayerService, IDisposable
    {
        private static IMediaPlayerService? instance;
        
        private static LibVLC LibVlc => Locator.LibVlc;

        public static IMediaPlayerService Instance => instance ??= new MediaPlayerService();

        private readonly MediaPlayer helperPlayer;
        
        public MediaPlayer MainPlayer { get; }
        
        private MediaPlayerService()
        {
            MainPlayer = new MediaPlayer(LibVlc);
            helperPlayer = new MediaPlayer(LibVlc);
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
            MainPlayer.Dispose();
        }
    }
}