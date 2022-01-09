namespace Footage.Views
{
    using System;
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using Footage.ViewModel;

    public partial class MainWindow : Window
    {
        // TODO remove singleton mainwindow
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