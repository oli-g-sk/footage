namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;
    using JetBrains.Annotations;

    public class VideoViewModel : EntityViewModel<Video>
    {
        public VideoViewModel([NotNull] Video item) : base(item)
        {
        }
    }
}