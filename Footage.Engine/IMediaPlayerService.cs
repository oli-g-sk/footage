namespace Footage.Engine
{
    using System.Threading.Tasks;

    public interface IMediaPlayerService
    {
        public long Duration { get; }

        Task LoadMedia(string uri);
        
        Task UnloadMedia();

        Task<long> GetVideoDuration(string videoUri);
    }
}