namespace Footage.UI.Views.MainWindowContent
{
    using Avalonia.Controls;
    using Avalonia.Interactivity;
    using Avalonia.Markup.Xaml;
    using Footage.ViewModel.Section;

    public class SourcesView : UserControl
    {
        private MediaSourcesViewModel ViewModel => DataContext as MediaSourcesViewModel;
        
        public SourcesView()
        {
            InitializeComponent();
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
            var task = dialog.ShowAsync(MainWindow.Instance);
            task.Wait();
            string? directory = task.Result;

            if (ViewModel.AddItemCommand.CanExecute(directory))
            {
                ViewModel.AddItemCommand.Execute(directory);
            }
        }
    }
}