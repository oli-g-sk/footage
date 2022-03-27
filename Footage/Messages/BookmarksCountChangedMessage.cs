namespace Footage.Messages
{
    using Footage.Model;

    public class BookmarksCountChangedMessage : EntityMessageBase<Video>
    {
        public int BookmarkCount { get; }
        
        public BookmarksCountChangedMessage(Video entity) : base(entity)
        {
            BookmarkCount = entity.Bookmarks.Count;
        }
    }
}