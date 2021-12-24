namespace Footage.ViewModel
{
    using Footage.Model;
    using JetBrains.Annotations;

    public class MediaSourceViewModel : EntityViewModel<MediaSource>
    {
        public string Name => Item.Name;

        // TODO include a second description line for other (future) source types
        public string Description =>
            (Item is LocalMediaSource localMediaSource) ? localMediaSource.RootPath : "Unknown source";
        
        public MediaSourceViewModel([NotNull] MediaSource item) : base(item)
        {
        }
    }
}