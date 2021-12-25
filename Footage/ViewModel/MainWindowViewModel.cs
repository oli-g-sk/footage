namespace Footage.ViewModel
{
    using System;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Section;

    public class MainWindowViewModel : SectionViewModel
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }
        
        public VideoDetailViewModel VideoDetail { get; }
        
        public MainWindowViewModel()
        {
            MediaSources = Locator.Get<MediaSourcesViewModel>();
            VideoBrowser = Locator.Get<VideoBrowserViewModel>();
            VideoDetail = Locator.Get<VideoDetailViewModel>();
        }
    }
}
