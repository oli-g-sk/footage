namespace Footage.Engine
{
    using System;
    using System.Threading.Tasks;

    public interface IMediaPlayer
    {
        event EventHandler PositionChanged;

        public float Position { get; set; }

        public long Duration { get; }

        public int Volume { get; set; }

        bool IsPlaying { get; }

        bool IsMediaLoaded { get; }

        Task Play();

        Task Pause();

        Task Stop();

        Task LoadMedia(string uri);
        
        Task UnloadMedia();

        Task<long> GetVideoDuration(string videoUri);
    }
}