namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;
    using JetBrains.Annotations;

    public class VideoViewModel : EntityViewModel<Video>
    {
        public long Duration => Item.Duration;
        
        public VideoViewModel([NotNull] Video item) : base(item)
        {
        }

        public int BookmarksCount => Item.Bookmarks.Count;
    }
}