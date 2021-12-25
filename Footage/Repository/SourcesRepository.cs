namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Service;

    public class SourcesRepository : RepositoryBase
    {
        public List<MediaSource> Sources { get; }
        
        public SourcesRepository()
        {
            // TODO load async
            Sources = dao.Query<MediaSource>().ToList();
        }
        
        public async Task<LocalMediaSource> AddLocalSource(string path, bool includeSubfolders)
        {
            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };
            
            Sources.Add(source);
            await dao.Insert(source);
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            Sources.Remove(source);
            await dao.Remove(source);
        }

        public async Task RefreshLocalSource(LocalMediaSource source)
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

            await dao.InsertRange(videos);
        }

        private async Task RemoveOrphanVideos(MediaSource removedSource)
        {
            throw new NotImplementedException();
        }
        
        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            return await dao.Contains<Video>(v => v.MediaSource == sourceVideoInfo.Source
                                                && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}