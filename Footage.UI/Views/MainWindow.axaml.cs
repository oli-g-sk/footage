namespace Footage.UI.Views
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Footage.Engine.LibVlc;
    using Footage.UI.EngineBindings;
    using Footage.ViewModel;
    using LibVLCSharp.Avalonia;

    public partial class MainWindow : Window
    {
        // TODO remove singleton mainwindow
        public static MainWindow Instance { get; private set; }

        private VideoView videoView;
        
        private MainWindowViewModel ViewModel => DataContext as MainWindowViewModel;

        private MediaPlayerService Player => ViewModel.Playback.Player as MediaPlayerService;

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
            videoView = this.FindControl<VideoView>("VideoViewLibVlc");
            EngineBindingLibVlc.Bind(Player, videoView);
        }
    }
}