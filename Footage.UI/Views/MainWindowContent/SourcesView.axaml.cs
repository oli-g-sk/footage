namespace Footage.UI.Views.MainWindowContent
{
    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using Footage.ViewModel.Section;

    public class SourcesView : UserControl
    {
        private MediaSourcesViewModel ViewModel => DataContext as MediaSourcesViewModel;
        
        public SourcesView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}