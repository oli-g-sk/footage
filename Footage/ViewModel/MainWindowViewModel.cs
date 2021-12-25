namespace Footage.ViewModel
{
    using System;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Section;

    public class MainWindowViewModel : SectionViewModel
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }
        
        public MainWindowViewModel()
        {
            MediaSources = Locator.Get<MediaSourcesViewModel>();
            VideoBrowser = Locator.Get<VideoBrowserViewModel>();
            MediaSources.SelectedSourceChanged += MediaSources_SelectedSourceChanged;
        }

        private void MediaSources_SelectedSourceChanged(object? sender, EventArgs e)
        {
            // TODO await / handle cancellation
            VideoBrowser.SwitchSource(MediaSources.SelectedSource);
        }
    }
}