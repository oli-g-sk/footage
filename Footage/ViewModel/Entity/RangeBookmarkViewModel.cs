namespace Footage.ViewModel.Entity
{
    using Footage.Model;

    public class RangeBookmarkViewModel : BookmarkViewModel
    {
        protected new RangeBookmark Item => (base.Item as RangeBookmark)!;

        public long StartTime
        {
            get => Item.StartTime;
            set
            {
                Item.StartTime = value;
                RaisePropertyChanged(nameof(StartTime));
                OnTimeChanged(value);
            }
        }

        public long EndTime
        {
            get => Item.EndTime;
            set
            {
                Item.EndTime = value;
                RaisePropertyChanged(nameof(EndTime));
                OnTimeChanged(value);
            }
        }
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public RangeBookmarkViewModel(RangeBookmark rangeBookmark) : base(rangeBookmark)
        {
        }
    }
}