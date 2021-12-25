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

    public class SourcesRepository : RepositoryBase
    {
        private readonly IEntityDao<MediaSource> mediaSourceDao;
        private readonly IEntityDao<Video> videoDao;

        public List<MediaSource> Sources { get; }
        
        public SourcesRepository()
        {
            mediaSourceDao = Locator.Dao<MediaSource>();
            videoDao = Locator.Dao<Video>();

            // TODO load async
            Sources = mediaSourceDao.Query().ToList();
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
            await mediaSourceDao.Insert(source);
            return source;
        }

        public async Task RemoveSource(MediaSource source)
        {
            Sources.Remove(source);
            await mediaSourceDao.Remove(source);
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
                    MediaSourceUri = sourceVideo.Identifier
                });
            }

            await videoDao.InsertRange(videos);
        }

        protected override IEnumerable<IDisposable> GetDisposables()
        {
            return new IDisposable[]
            {
                videoDao,
                mediaSourceDao
            };
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