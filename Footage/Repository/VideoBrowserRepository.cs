namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        private IMediaPlayerService mediaPlayerService => new MediaPlayerService();
        
        // TODO make async
        public async Task<IEnumerable<Video>> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            using var dao = new EntityDao();

            var videos = dao.Query<Video>(v => v.MediaSource == selectedSource)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);
            
            return await videos.ToListAsync();
        }

        public async Task UpdateVideoDuration(MediaSource mediaSource, Video video)
        {
            var mediaProvider = MediaProviderBase.GetMediaProvider(mediaSource);

            string path = mediaProvider.GetFullPath(video);
            video.Duration = await mediaPlayerService.GetVideoDuration(path);
#if DEBUG
            // await Task.Delay(300);
#endif

            using var dao = new EntityDao();
            await dao.Update(video);
            await dao.Commit();
        }
    }
    
}
