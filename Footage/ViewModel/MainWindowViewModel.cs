namespace Footage.ViewModel
{
    public class MainWindowViewModel
    {
        public MediaSourcesViewModel MediaSources { get; }
        
        public MainWindowViewModel(MediaSourcesViewModel mediaSourcesViewModel)
        {
            MediaSources = mediaSourcesViewModel;
        }
    }
}