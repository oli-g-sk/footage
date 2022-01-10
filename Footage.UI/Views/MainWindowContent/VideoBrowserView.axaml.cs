namespace Footage.UI.Views.MainWindowContent
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

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