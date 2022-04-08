using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Footage.Application.ViewModel.Section;

namespace Footage.Application.UI.Views.MainWindowContent
{
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