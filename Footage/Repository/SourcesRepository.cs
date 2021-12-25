namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;

    public class SourcesRepository
    {
        private readonly IMediaSourceDao mediaSourceDao;
        private readonly IVideoDao videoDao;

        public List<MediaSource> Sources { get; }
        
        public SourcesRepository(IMediaSourceDao mediaSourceDao, IVideoDao videoDao)
        {
            this.mediaSourceDao = mediaSourceDao;
            this.videoDao = videoDao;

            // TODO load async
            Sources = mediaSourceDao.Query().ToList();
        }
        
        public LocalMediaSource AddLocalSource(string path, bool includeSubfolders)
        {
            var source = new LocalMediaSource
            {
                RootPath = path,
                IncludeSubfolders = includeSubfolders,
                Name = Path.GetFileName(path)
            };
            
            Sources.Add(source);
            mediaSourceDao.Insert(source);
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            Sources.Remove(source);
            mediaSourceDao.Remove(source);
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
                    MediaSourceId = source.Id,
                });
            }

#if DEBUG
            await Task.Delay(4000);
#endif
            
            await videoDao.InsertRange(videos);
        }

        private async Task RemoveOrphanVideos(MediaSource removedSource)
        {
            throw new NotImplementedException();
        }
        
        private async Task<bool> VideoAlreadyImported(SourceVideoInfo sourceVideoInfo)
        {
            return await videoDao.Contains(v => v.MediaSourceId == sourceVideoInfo.Source.Id
                                                && v.MediaSourceUri == sourceVideoInfo.Identifier);
        }
    }
}