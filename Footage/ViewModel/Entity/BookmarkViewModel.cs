namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;

    public abstract class BookmarkViewModel : EntityViewModel<Bookmark>
    {
        public bool IsRange => Item is RangeBookmark;

        public long VideoDuration => Item.Video.Duration;

        public BookmarkViewModel(Bookmark bookmark) : base(bookmark)
        {
        }
    }
}