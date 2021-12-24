using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Instance = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}