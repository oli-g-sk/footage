namespace Footage.UI.Views.MainWindowContent
{
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;

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