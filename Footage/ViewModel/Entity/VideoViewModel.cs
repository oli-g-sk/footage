namespace Footage.ViewModel.Entity
{
    using System;
    using Footage.Messages;
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class VideoViewModel : EntityViewModel<Video>
    {
        private long duration;
        
        public TimeSpan Duration => TimeSpan.FromMilliseconds(duration);
        
        public VideoViewModel(Video item) : base(item)
        {
            IsMissing = item.IsMissing;
            duration = item.Duration;
            BookmarksCount = item.Bookmarks.Count;
            
            RegisterToEntityMessage<VideoMetadataUpdatedMessage>(OnVideoMetadataUpdated);
            RegisterToEntityMessage<BookmarksCountChangedMessage>(OnBookmarksCountChanged);
        }

        private int bookmarksCount;
        public int BookmarksCount
        {
            get => bookmarksCount;
            set => Set(ref bookmarksCount, value);
        }

        private bool isMissing;

        public bool IsMissing
        {
            get => isMissing;
            set => Set(ref isMissing, value);
        }

        private void OnVideoMetadataUpdated(VideoMetadataUpdatedMessage message)
        {
            if (message.Id == Id)
            {
                IsMissing = message.IsMissing;
                duration = message.Duration;
                RaisePropertyChanged(nameof(Duration));
            }
        }
        
        private void OnBookmarksCountChanged(BookmarksCountChangedMessage message)
        {
            if (message.Id == Id)
            {
                BookmarksCount = message.BookmarkCount;
            }
        }
    }
}