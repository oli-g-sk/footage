namespace Footage.Application.ViewModel
{
    using System;
    using Footage.Application.ViewModel.Base;
    using Footage.Application.ViewModel.Section;

    public sealed class MainWindowViewModel : SectionViewModel, IDisposable
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public ProjectsViewModel Projects { get; }

        public VideoBrowserViewModel VideoBrowser { get; }

        public PlaybackViewModel Playback { get; }

        public BookmarksViewModel Bookmarks { get; }

        public MainWindowViewModel()
        {
            MediaSources = new MediaSourcesViewModel();
            Projects = new ProjectsViewModel();
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
