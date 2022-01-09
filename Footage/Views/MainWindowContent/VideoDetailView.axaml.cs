using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.Views.MainWindowContent
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