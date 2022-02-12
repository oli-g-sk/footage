using Footage.Engine;
using Footage.Model;
using Footage.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Repository
{
    public class VideoDetailRepository : RepositoryBase
    {
        // TODO replace by future MediaInfoService, playback is not needed here
        private readonly IMediaPlayer mediaPlayer;
        private readonly ISourceScopedServiceFactory sourceScopedServiceFactory;

        public VideoDetailRepository(ISourceScopedServiceFactory sourceScopedServiceFactory)
        {
            this.mediaPlayer = Locator.Create<IMediaPlayer>();
            this.sourceScopedServiceFactory = sourceScopedServiceFactory;
        }

        public string GetVideoPath(MediaSource mediaSource, Video video)
        {
            var mediaProvider = sourceScopedServiceFactory.GetMediaProviderService(mediaSource);
            return mediaProvider.GetFullPath(video);
        }

        public async Task ProcessSelectedVideo(MediaSource mediaSource, Video video)
        {
            // TODO detect video changes (check if duration (and other metadata) match what's stored in DB)

            string path = GetVideoPath(mediaSource, video);

            if (!File.Exists(path))
            {
                // TODO check if video file path is readable
                video.IsMissing = true;
            }
            else
            {
                video.Duration = await mediaPlayer.GetVideoDuration(path);
            }
#if DEBUG
            // await Task.Delay(300);
#endif

            using var dao = GetDao();
            await dao.Update(video);
            await dao.Commit();
        }
    }
}
