namespace Footage.ViewModel.Entity
{
    using Footage.Model;

    public class TimeBookmarkViewModel : BookmarkViewModel
    {
        protected new TimeBookmark Item => (base.Item as TimeBookmark)!;
        
        public long? Time => Item.Time;
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public TimeBookmarkViewModel(TimeBookmark timeBookmark) : base(timeBookmark)
        {
        }
    }
}