using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage
{
    using Footage.ViewModel;
    using LibVLCSharp.Shared;

    public partial class MainWindow : Window
    {
        // TODO REMOVE
        public static MainWindow Instance { get; private set; }
        
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Instance = this;
        }

        public void SetViewModel(MainWindowViewModel viewModel)
        {
            DataContext = viewModel;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}