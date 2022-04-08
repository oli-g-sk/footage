namespace Footage.Application.ViewModel.Entity
{
    using System;
    using Footage.Application.ViewModel.Base;
    using Footage.Model;

    public abstract class BookmarkViewModel : EntityViewModel<Bookmark>
    {
        public bool IsRange => Item is RangeBookmark;

        public long VideoDuration => Item.Video.Duration;

        public BookmarkPriority Priority
        {
            get => Item.Priority;
            set
            {
                Item.Priority = value;
                RaisePropertyChanged(nameof(Priority));
            }
        }

        public event Action<long> TimeChanged; 

        public BookmarkViewModel(Bookmark bookmark) : base(bookmark)
        {
        }

        protected void OnTimeChanged(long newTime)
        {
            TimeChanged?.Invoke(newTime);
        }
    }
}