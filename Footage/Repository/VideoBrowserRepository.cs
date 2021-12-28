namespace Footage.Repository
{
    using System.Collections.Generic;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        // TODO make async
        public IEnumerable<Video> FetchVideos(MediaSource selectedSource, int? batchSize = null)
        {
            // TODO use batch size limit
            
            var videos = Dao.Query<Video>(v => v.MediaSource == selectedSource)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);
            
            return videos;
        }
    }
    
}