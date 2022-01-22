using Footage.Model;
using Footage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Repository
{
    public class VideoDetailRepository : RepositoryBase
    {
        private readonly IMediaPlayerService mediaPlayerService;
        private readonly IMediaProviderFactory mediaProviderFactory;

        public VideoDetailRepository(IMediaPlayerService mediaPlayerService,
            IMediaProviderFactory mediaProviderFactory)
        {
            this.mediaPlayerService = mediaPlayerService;
            this.mediaProviderFactory = mediaProviderFactory;
        }

        public string GetVideoPath(MediaSource mediaSource, Video video)
        {
            var mediaProvider = mediaProviderFactory.GetMediaProvider(mediaSource);
            return mediaProvider.GetFullPath(video);
        }

        public async Task ProcessSelectedVideo(MediaSource mediaSource, Video video)
        {
            // TODO detect video changes (check if duration (and other metadata) match what's stored in DB)

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
