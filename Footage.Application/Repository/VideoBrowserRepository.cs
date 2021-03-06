namespace Footage.Application.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Model.ModelHelper;
    using Microsoft.EntityFrameworkCore;

    public class VideoBrowserRepository : RepositoryBase
    {
        private int currentPage;

        private int mediaSourceId;
        private BookmarkFilter bookmarkFilter;
        
#if DEBUG
        private const int BatchSize = 5;
#else
        private const int BatchSize = 100;
#endif
        
        public async Task UpdateVideoQuery(int selectedMediaSourceId, BookmarkFilter bookmarkFilter)
        {
            currentPage = 0;
            
            mediaSourceId = selectedMediaSourceId;
            this.bookmarkFilter = bookmarkFilter;

            await Task.CompletedTask;
        }

        public async Task<IEnumerable<Video>> Fetch()
        {
            using var dao = GetDao();
            var query = dao.Query<Video>(v => v.MediaSourceId == mediaSourceId);
            
            // apply filters
            if (bookmarkFilter?.Enabled ?? false)
            {
                query = query.Where(v => 
                    (bookmarkFilter.IncludeLow && v.Bookmarks.Any(b => b.Priority == BookmarkPriority.Low))
                 || (bookmarkFilter.IncludeMedium && v.Bookmarks.Any(b => b.Priority == BookmarkPriority.Medium))
                 || (bookmarkFilter.IncludeHigh && v.Bookmarks.Any(b => b.Priority == BookmarkPriority.High)));
            }

            // resolve navigation properties
            query = query.Include(v => v.MediaSource)
                .Include(v => v.Bookmarks);

            var batch = query.Skip(currentPage * BatchSize)
                .Take(BatchSize);
            
            currentPage++;
            
            return await batch.ToListAsync();
        }
    }
    
}