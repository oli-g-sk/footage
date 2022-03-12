
namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service;
    using Footage.Service.SourceScoped;
    using NLog;
    using NLog.Fluent;

    public class LibraryRepository : RepositoryBase
    {
        private static ILogger Log => LogManager.GetCurrentClassLogger();
        
        private readonly ISourceScopedServiceFactory sourceScopedServiceFactory;
        
        public LibraryRepository(ISourceScopedServiceFactory sourceScopedServiceFactory)
        {
            this.sourceScopedServiceFactory = sourceScopedServiceFactory;
        }
        
        public async Task ImportNewFiles(int mediaSourceId)
        {
            using var dao = GetDao();
            var source = await dao.Get<MediaSource>(mediaSourceId);
            
            var provider = sourceScopedServiceFactory.GetMediaProviderService(source);
            var mediaInfo = sourceScopedServiceFactory.GetMediaInfoService(source);

            Log.Debug($"Scanning for new videos in source {source}.");
            var sourceVideos = provider.FetchVideos();
            var videos = new List<Video>();

            foreach (var sourceVideo in sourceVideos)
            {
                if (await VideoAlreadyImported(sourceVideo))
                {
                    continue;
                }
                
                var video = new Video
                {
                    MediaSource = source,
                    MediaSourceUri = sourceVideo.Identifier
                };
                
                videos.Add(video);
            }

            Log.Info($"Number of new files in source '{source.Name}': {videos.Count}.");

            if (!videos.Any())
            {
                return;
            }
            
            await dao.InsertRange(videos);
            await dao.Commit();
            Log.Debug($"New files saved to DB in source '{source.Name}'.");

            foreach (var video in videos)
            {
                // TODO move to a shared method in order to update media info in one consistent way
                // (here, when loaded to player, etc)
                var videoUri = provider.GetFullPath(video);
                video.Duration = await mediaInfo.GetDuration(videoUri);
                
                // TODO FOO-33 FOO-22 scan media info in background for ALL videos, not just newly imported (for potential changes)
            }

            Log.Debug("Media info for new videos updated.");

            await dao.UpdateRange(videos);
            await dao.Commit();
        }

        public async Task<int> GetVideoCount(int mediaSourceId)
        {
            using var dao = GetDao();
            var source = await dao.Get<MediaSource>(mediaSourceId);
            return dao.Query<Video>(v => v.MediaSource == source).Count();
        }

        private bool MediaInfoLoaded(Video video)
        {
            return video.Duration > 0;
        }
        
        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            using var dao = GetDao();
            return await dao.Contains<Video>(v => v.MediaSource == sourceVideoInfo.Source
                                                  && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}
