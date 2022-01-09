namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        private IMediaPlayerService mediaPlayerService => MediaPlayerService.Instance;
        
        // TODO make async
        public IEnumerable<Video> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            
            var videos = Dao.Query<Video>(v => v.MediaSource == selectedSource)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);
            
            return videos;
        }

        public async Task UpdateVideoDuration(MediaSource mediaSource, Video video)
        {
            var mediaProvider = MediaProviderBase.GetMediaProvider(mediaSource);

            string path = mediaProvider.GetFullPath(video);
            video.Duration = await mediaPlayerService.GetVideoDuration(path);
#if DEBUG
            // await Task.Delay(300);
#endif
            
            await Dao.Update(video);
        }
    }
    
}
