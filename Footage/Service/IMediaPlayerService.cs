namespace Footage.Service
{
    using System.Threading.Tasks;
    using LibVLCSharp.Shared;

    public interface IMediaPlayerService
    {
        static IMediaPlayerService Instance { get; }
        
        public MediaPlayer MainPlayer { get; }

        Task<long> GetVideoDuration(string videoUri);
    }
}