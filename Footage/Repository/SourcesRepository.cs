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
        public async Task<LocalMediaSource> AddLocalSource(string path, bool includeSubfolders)
        {
            using var dao = new EntityDao();

            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };
            
            await dao.Insert(source);
            await dao.Commit();

            await ImportNewFiles(source);
            
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            using var dao = new EntityDao();
            await dao.Remove(source);
            await dao.Commit();
        }

        public async Task ImportNewFiles(LocalMediaSource source)
        {
            var provider = new LocalMediaProvider(source);
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

            using var dao = new EntityDao();
            await dao.InsertRange(videos);
            await dao.Commit();
        }

        public async Task<IEnumerable<MediaSource>> GetAllSources()
        {
            using var dao = new EntityDao();
            return await dao.Query<MediaSource>().ToListAsync();
        }

        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            using var dao = new EntityDao();
            return await dao.Contains<Video>(v => v.MediaSource == sourceVideoInfo.Source
                                                && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}