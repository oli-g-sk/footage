namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using Footage.Dao;
    using Footage.Model;

    public class VideoBrowserRepository : RepositoryBase
    {
        private readonly IEntityDao<Video> videoDao;

        public VideoBrowserRepository()
        {
            videoDao = Locator.Dao<Video>();
        }
        
        // TODO make async
        public IEnumerable<Video> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            var videos = videoDao.Query(v => v.MediaSourceId == selectedSource.Id);
            return videos;
        }

        protected override IEnumerable<IDisposable> GetDisposables()
        {
            return new[] { videoDao };
        }
    }
    
}