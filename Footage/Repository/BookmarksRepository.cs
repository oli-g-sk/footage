
namespace Footage.Repository
{
    using System;
    using System.Threading.Tasks;
    using Footage.Model;

    public class BookmarksRepository : RepositoryBase
    {
        public async Task<TimeBookmark> AddTimeBookmarkToVideo(Video video, long position)
        {
            var bookmark = new TimeBookmark
            {
                Time = position,
                Video = video
            };
            
            video.Bookmarks.Add(bookmark);
            
            await Dao.Update(video);
            await Dao.Commit();
            return bookmark;
        }
    }
}
