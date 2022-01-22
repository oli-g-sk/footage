namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;
    using JetBrains.Annotations;

    public class VideoViewModel : EntityViewModel<Video>
    {
        public long Duration => Item.Duration;
        
        public VideoViewModel(Video item) : base(item)
        {
        }

        public int BookmarksCount => Item.Bookmarks.Count;

        public bool IsMissing => Item.IsMissing;
    }
}