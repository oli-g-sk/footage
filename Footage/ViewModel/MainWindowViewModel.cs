namespace Footage.ViewModel
{
    using System;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Section;

    public sealed class MainWindowViewModel : SectionViewModel, IDisposable
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }
        
        public VideoDetailViewModel VideoDetail { get; }

        public MainWindowViewModel()
        {
            MediaSources = new MediaSourcesViewModel();
            VideoBrowser = new VideoBrowserViewModel();
            VideoDetail = new VideoDetailViewModel();
        }

        public void Dispose()
        {
            // TODO dispose of VideoDetail -> Playback ?
        }
    }
}
