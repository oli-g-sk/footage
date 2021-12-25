namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using Footage.Dao;
    using Footage.Model;

    public class VideoBrowserRepository : RepositoryBase
    {
        // TODO make async
        public IEnumerable<Video> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            var allVideos = dao.Query<Video>(v => true);
            var videos = dao.Query<Video>(v => v.MediaSource == selectedSource);
            return videos;
        }
    }
    
}