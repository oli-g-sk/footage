namespace Footage.ViewModel.Entity
{
    using Footage.Model;

    public class RangeBookmarkViewModel : BookmarkViewModel
    {
        protected new RangeBookmark Item => (base.Item as RangeBookmark)!;

        public long StartTime => Item.StartTime;

        public long EndTime => Item.EndTime;
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public RangeBookmarkViewModel(RangeBookmark rangeBookmark) : base(rangeBookmark)
        {
        }
    }
}