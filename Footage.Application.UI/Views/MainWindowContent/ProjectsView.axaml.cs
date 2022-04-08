using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.Application.UI.Views.MainWindowContent
{
    public class ProjectsView : UserControl
    {
        public ProjectsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}