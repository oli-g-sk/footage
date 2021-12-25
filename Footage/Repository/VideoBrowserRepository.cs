namespace Footage.Repository
{
    using System.Collections.Generic;
    using Footage.Dao;
    using Footage.Model;

    public class VideoBrowserRepository
    {
        private readonly IVideoDao videoDao;

        public VideoBrowserRepository(IVideoDao videoDao)
        {
            this.videoDao = videoDao;
        }
        
        // TODO make async
        public IEnumerable<Video> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit

            var videos = videoDao.Query(v => v.MediaSourceId == selectedSource.Id);
            return videos;
        }
    }
    
}