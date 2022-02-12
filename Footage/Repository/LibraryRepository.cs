
namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service;
    using Footage.Service.SourceScoped;

    public class LibraryRepository : RepositoryBase
    {
        private readonly ISourceScopedServiceFactory sourceScopedServiceFactory;
        
        public LibraryRepository(ISourceScopedServiceFactory sourceScopedServiceFactory)
        {
            this.sourceScopedServiceFactory = sourceScopedServiceFactory;
        }
        
        public async Task ImportNewFiles(MediaSource source)
        {
            var provider = sourceScopedServiceFactory.GetMediaProviderService(source);
            var sourceVideos = provider.FetchVideos();

            var videos = new List<Video>();

            foreach (var sourceVideo in sourceVideos)
            {
                if (await VideoAlreadyImported(sourceVideo))
                {
                    continue;
                }
                
                videos.Add(new Video
                {
                    MediaSource = source,
                    MediaSourceUri = sourceVideo.Identifier
                });
            }

            using var dao = GetDao();
            await dao.InsertRange(videos);
            await dao.Commit();
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
