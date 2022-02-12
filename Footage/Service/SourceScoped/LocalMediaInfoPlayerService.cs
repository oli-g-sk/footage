namespace Footage.Service.SourceScoped
{
    using System.Threading.Tasks;
    using Footage.Engine;
    using Footage.Model;

    /// <summary>
    /// A basic implementation of <see cref="IMediaInfoService"/> which actually
    /// loads a file into a <see cref="IMediaPlayer"/> to provide media info.
    /// </summary>
    public class LocalMediaInfoPlayerService : IMediaInfoService
    {
        private IMediaPlayer player;

        public LocalMediaInfoPlayerService()
        {
            player = Locator.Create<IMediaPlayer>();
        }
        
        public async Task<long> GetDuration(string uri)
        {
            await player.LoadMedia(uri);
            long duration = player.Duration;
            await player.UnloadMedia();
            return duration;
        }
    }
}