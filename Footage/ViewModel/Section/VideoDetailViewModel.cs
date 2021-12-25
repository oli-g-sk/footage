namespace Footage.ViewModel.Section
{
    using Footage.ViewModel.Base;

    public class VideoDetailViewModel : SectionViewModel
    {
        public PlaybackViewModel Playback { get; }

        public VideoDetailViewModel()
        {
            Playback = new PlaybackViewModel();
        }
    }
}