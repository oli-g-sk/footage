namespace Footage.ViewModel.Entity
{
    using System;
    using System.Drawing;
    using Footage.Messages;
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class VideoViewModel : EntityViewModel<Video>
    {
        private long duration;
        private Image? thumbnail;
        
        public TimeSpan Duration => TimeSpan.FromMilliseconds(duration);

        public Image? Thumbnail
        {
            get => thumbnail;
            set => Set(ref thumbnail, value);
        }
        
        public VideoViewModel(Video item) : base(item)
        {
            IsMissing = item.IsMissing;
            duration = item.Duration;
            BookmarksCount = item.Bookmarks.Count;
            
            MessengerHelper.Register<VideoMetadataUpdatedMessage>(this, OnVideoMetadataUpdated);
            MessengerHelper.Register<BookmarksCountChangedMessage>(this, OnBookmarksCountChanged);
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
            IsMissing = message.IsMissing;
            duration = message.Duration;
            RaisePropertyChanged(nameof(Duration));
        }
        
        private void OnBookmarksCountChanged(BookmarksCountChangedMessage message)
        {
            BookmarksCount = message.BookmarkCount;
        }
    }
}