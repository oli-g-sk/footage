namespace Footage.Application.ViewModel.Entity
{
    using Footage.Model;

    public class TimeBookmarkViewModel : BookmarkViewModel
    {
        protected new TimeBookmark Item => (base.Item as TimeBookmark)!;

        public long Time
        {
            get => Item.Time;
            set
            {
                Item.Time = value;
                RaisePropertyChanged(nameof(Time));
                OnTimeChanged(value);
            }
        }
        
        // ReSharper disable once SuggestBaseTypeForParameter
        public TimeBookmarkViewModel(TimeBookmark timeBookmark) : base(timeBookmark)
        {
        }
    }
}