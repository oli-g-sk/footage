namespace Footage.ViewModel.Entity
{
    using System;
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class VideoViewModel : EntityViewModel<Video>
    {
        public TimeSpan Duration => TimeSpan.FromMilliseconds(Item.Duration);
        
        public VideoViewModel(Video item) : base(item)
        {
        }

        public int BookmarksCount => Item.Bookmarks.Count;

        public bool IsMissing => Item.IsMissing;
    }
}