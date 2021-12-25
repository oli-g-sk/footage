namespace Footage.ViewModel
{
    using System;
    using Footage.ViewModel.Section;

    public class MainWindowViewModel
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public VideoBrowserViewModel VideoBrowser { get; }
        
        public MainWindowViewModel(MediaSourcesViewModel mediaSourcesViewModel, VideoBrowserViewModel videoBrowserViewModel)
        {
            MediaSources = mediaSourcesViewModel;
            VideoBrowser = videoBrowserViewModel;
            MediaSources.SelectedSourceChanged += MediaSources_SelectedSourceChanged;
        }

        private void MediaSources_SelectedSourceChanged(object? sender, EventArgs e)
        {
            // TODO await / handle cancellation
            VideoBrowser.SwitchSource(MediaSources.SelectedSource);
        }
    }
}