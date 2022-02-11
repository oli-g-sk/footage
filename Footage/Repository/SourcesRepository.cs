namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class SourcesRepository : RepositoryBase
    {
        private IMediaProviderFactory mediaProviderFactory;

        public SourcesRepository(IMediaProviderFactory mediaProviderFactory)
        {
            this.mediaProviderFactory = mediaProviderFactory;
        }
        
        public async Task<LocalMediaSource> AddLocalSource(string path, bool includeSubfolders)
        {
            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };

            await ImportNewFiles(source);
            
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            using var dao = GetDao();
            await dao.Remove(source);
            await dao.Commit();
        }

        public async Task ImportNewFiles(MediaSource source)
        {
            var provider = mediaProviderFactory.GetMediaProvider(source);
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

        public async Task<IEnumerable<MediaSource>> GetAllSources()
        {
            using var dao = GetDao();
            return await dao.Query<MediaSource>().ToListAsync();
        }

        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            using var dao = GetDao();
            return await dao.Contains<Video>(v => v.MediaSource == sourceVideoInfo.Source
                                                && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}