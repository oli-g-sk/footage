﻿
namespace Footage.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Dao;
    using Footage.Messages;
    using Footage.Model;
    using GalaSoft.MvvmLight.Messaging;

    public class BookmarksRepository : RepositoryBase
    {
        public async Task<TimeBookmark> AddTimeBookmarkToVideo(Video video, long position)
        {
            using var dao = GetDao();

            var bookmark = new TimeBookmark
            {
                Time = position,
                Video = video,
                Priority = BookmarkPriority.Medium,
            };
            
            video.Bookmarks.Add(bookmark);
            
            await dao.Update(video);
            await dao.Commit();
            
            Messenger.Default.Send(new BookmarksCountChangedMessage(video));

            return bookmark;
        }

        public async Task RemoveBookmarks(Video video, IEnumerable<Bookmark> bookmarks)
        {
            using var dao = GetDao();

            foreach (var bookmark in bookmarks)
            {
                await dao.Remove(bookmark);
                video.Bookmarks.Remove(bookmark);
                // TODO use RemoveRange
            }

            await dao.Update(video);
            await dao.Commit();
            
            Messenger.Default.Send(new BookmarksCountChangedMessage(video));
        }

        public async Task UpdateBookmarkTimes(IEnumerable<Bookmark> bookmarks)
        {
            using var dao = GetDao();
            await dao.UpdateRange(bookmarks);
            await dao.Commit();
        }
    }
}
