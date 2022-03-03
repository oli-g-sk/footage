namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Model;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        private int currentPage;

        private int mediaSourceId;
        
#if DEBUG
        private const int BatchSize = 5;
#else
        private const int BatchSize = 100;
#endif
        
        public async Task UpdateVideoQuery(int selectedMediaSourceId)
        {
            currentPage = 0;
            
            mediaSourceId = selectedMediaSourceId;
            // TODO apply filters
            
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Video>> Fetch()
        {
            using var dao = GetDao();
            var query = dao.Query<Video>(v => v.MediaSourceId == mediaSourceId)
                .Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);

            var batch = query.Skip(currentPage * BatchSize)
                .Take(BatchSize);
            
            currentPage++;
            
            return await batch.ToListAsync();
        }
    }
    
}
