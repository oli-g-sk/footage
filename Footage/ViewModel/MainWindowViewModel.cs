namespace Footage.ViewModel
{
    using System;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Section;
    using GalaSoft.MvvmLight.Command;

    public sealed class MainWindowViewModel : SectionViewModel, IDisposable
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }
        
        public VideoDetailViewModel VideoDetail { get; }
        
        public SelectedVideoViewModel SelectedVideo { get; }

        public MainWindowViewModel()
        {
            SelectedVideo = new SelectedVideoViewModel();
            
            MediaSources = new MediaSourcesViewModel();
            VideoBrowser = new VideoBrowserViewModel();
            VideoDetail = new VideoDetailViewModel(SelectedVideo);
        }

        public void Dispose()
        {
            SelectedVideo.Dispose();
        }
    }
}
