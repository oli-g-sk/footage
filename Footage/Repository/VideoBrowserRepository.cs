namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Footage.Service;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        // TODO make async
        public async Task<IEnumerable<Video>> FetchVideos(int selectedMediaSourceId, int? batchSize = null)
        {
            // TODO use batch size limit
            using var dao = GetDao();
            var source = await dao.Get<MediaSource>(selectedMediaSourceId);

            var videos = dao.Query<Video>(v => v.MediaSource == source)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);
            
            return await videos.ToListAsync();
        }
    }
    
}
