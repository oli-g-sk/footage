namespace Footage.Application.Messages
{
    using Footage.Model;

    public class BookmarksCountChangedMessage : EntityMessageBase
    {
        public int BookmarkCount { get; }
        
        public BookmarksCountChangedMessage(Video entity) : base(entity)
        {
            BookmarkCount = entity.Bookmarks.Count;
        }
    }
}