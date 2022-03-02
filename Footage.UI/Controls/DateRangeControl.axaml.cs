using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage.UI.Controls
{
    using System.Diagnostics;
    using Avalonia.Controls.Primitives;

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

        private void Slider_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            var slider = sender as Slider;

            if (e.Property == RangeBase.ValueProperty)
            {
                Debug.WriteLine(e.NewValue);
            }
        }
    }
}