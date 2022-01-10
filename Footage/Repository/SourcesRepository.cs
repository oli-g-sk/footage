namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class SourcesRepository : RepositoryBase
    {
        public async Task<LocalMediaSource> AddLocalSource(string path, bool includeSubfolders)
        {
            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };
            
            await Dao.Insert(source);
            await Dao.Commit();

            await ImportNewFiles(source);
            
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            await Dao.Remove(source);
            await Dao.Commit();
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

            await Dao.InsertRange(videos);
            await Dao.Commit();
        }

        public async Task<IEnumerable<MediaSource>> GetAllSources()
        {
            return await Dao.Query<MediaSource>().ToListAsync();
        }

        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            return await Dao.Contains<Video>(v => v.MediaSource == sourceVideoInfo.Source
                                                && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}