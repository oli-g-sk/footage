using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Footage
{
    using Avalonia.Interactivity;
    using Footage.ViewModel;
    using LibVLCSharp.Shared;

    public partial class MainWindow : Window
    {
        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;
        
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void BtnAddMediaSource_OnClick(object? sender, RoutedEventArgs e)
        {
            // TODO move to VM layer utilizing a middleware DIALOG SERVICE
            var dialog = new OpenFolderDialog();
            
            // TODO make async
            var task = dialog.ShowAsync(this);
            task.Wait();
            string? directory = task.Result;

            if (ViewModel.MediaSources.AddItemCommand.CanExecute(directory))
            {
                ViewModel.MediaSources.AddItemCommand.Execute(directory);
            }
        }
    }
}