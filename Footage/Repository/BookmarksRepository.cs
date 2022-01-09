
namespace Footage.Repository
{
    using System;
    using System.Collections.Generic;
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

        public async Task RemoveBookmarks(Video video, IEnumerable<Bookmark> bookmarks)
        {
            foreach (var bookmark in bookmarks)
            {
                await Dao.Remove(bookmark);
                video.Bookmarks.Remove(bookmark);
                // TODO use RemoveRange
            }

            await Dao.Update(video);
            await Dao.Commit();
        }

        public async Task UpdateBookmarkTimes(IEnumerable<Bookmark> bookmarks)
        {
            await Dao.UpdateRange(bookmarks);
            await Dao.Commit();
        }
    }
}
