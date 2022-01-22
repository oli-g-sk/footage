namespace Footage.ViewModel
{
    using System;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Section;

    public sealed class MainWindowViewModel : SectionViewModel, IDisposable
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }

        public PlaybackViewModel Playback { get; }

        public BookmarksViewModel Bookmarks { get; }

        public MainWindowViewModel()
        {
            MediaSources = new MediaSourcesViewModel();
            VideoBrowser = new VideoBrowserViewModel();
            Playback = new PlaybackViewModel();
            Bookmarks = new BookmarksViewModel();
        }

        public void Dispose()
        {
            // TODO dispose of VideoDetail -> Playback ?
        }
    }
}
