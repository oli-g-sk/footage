using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.UI.Controls
{
    public class DateRangeControl : UserControl
    {
        public DateRangeControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}