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

        public async Task<string> GetVideoPath(int videoId)
        {
            using var dao = GetDao();
            var video = await dao.Get<Video>(videoId);
            var mediaProvider = sourceScopedServiceFactory.GetMediaProviderService(video.MediaSource);
            return mediaProvider.GetFullPath(video);
        }

        public async Task ProcessVideo(int videoId)
        {
            using var dao = GetDao();
            
            var video = await dao.Get<Video>(videoId);
            string path = await GetVideoPath(videoId);

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
            
            await dao.Update(video);
            await dao.Commit();
        }
    }
}
