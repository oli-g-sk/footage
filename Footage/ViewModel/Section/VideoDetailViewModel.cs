namespace Footage.ViewModel.Section
{
    using Footage.ViewModel.Base;

    public class VideoDetailViewModel : SectionViewModel
    {
        public PlaybackViewModel Playback { get; }
        
        public BookmarksViewModel Bookmarks { get; }

        public VideoDetailViewModel(SelectedVideoViewModel selectedVideoViewModel)
        {
            Playback = new PlaybackViewModel();
            Bookmarks = new BookmarksViewModel(selectedVideoViewModel);
        }
    }
}