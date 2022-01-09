using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.Views.MainWindowContent
{
    public class VideoBrowserView : UserControl
    {
        public VideoBrowserView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}