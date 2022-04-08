using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.Application.UI.Views.MainWindowContent
{
    public class VideoDetailView : UserControl
    {
        public VideoDetailView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}