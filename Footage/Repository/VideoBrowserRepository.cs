namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        private readonly IMediaPlayerService mediaPlayerService;
        private readonly IMediaProviderFactory mediaProviderFactory;

        public VideoBrowserRepository(IMediaPlayerService mediaPlayerService,
            IMediaProviderFactory mediaProviderFactory)
        {
            this.mediaPlayerService = mediaPlayerService;
            this.mediaProviderFactory = mediaProviderFactory;
        }

        // TODO make async
        public async Task<IEnumerable<Video>> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            using var dao = GetDao();

            var videos = dao.Query<Video>(v => v.MediaSource == selectedSource)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);
            
            return await videos.ToListAsync();
        }

        public string GetVideoPath(MediaSource mediaSource, Video video)
        {
            var mediaProvider = mediaProviderFactory.GetMediaProvider(mediaSource);
            return mediaProvider.GetFullPath(video);
        }

        public async Task UpdateVideoDuration(MediaSource mediaSource, Video video)
        {
            string path = GetVideoPath(mediaSource, video);
            video.Duration = await mediaPlayerService.GetVideoDuration(path);
#if DEBUG
            // await Task.Delay(300);
#endif

            using var dao = GetDao();
            await dao.Update(video);
            await dao.Commit();
        }
    }
    
}
