namespace Footage.Application.ViewModel.Entity
{
    using Footage.Application.ViewModel.Base;
    using Footage.Model;

    public class MediaSourceViewModel : NamedEntityViewModel<MediaSource>
    {
        private int videoCount;

        public int VideoCount
        {
            get => videoCount;
            set => Set(ref videoCount, value);
        }

        // TODO include a second description line for other (future) source types
        public string Description =>
            (Item is LocalMediaSource localMediaSource) ? localMediaSource.RootPath : "Unknown source";

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => Set(ref isBusy, value);
        }

        public bool IsActive
        {
            get => Item.Active;
            set
            {
                Item.Active = value;
                RaisePropertyChanged(nameof(IsActive));
            }
        }
        
        public MediaSourceViewModel(MediaSource item) : base(item)
        {
        }
    }
}