namespace Footage.UI.Views
{
    using Avalonia;
    using Avalonia.Controls;
    using Avalonia.Markup.Xaml;
    using Footage.Engine.MediaPlayer.LibVlc;
    using Footage.UI.EngineBindings;
    using Footage.ViewModel;
    using LibVLCSharp.Avalonia;

    public partial class MainWindow : Window
    {
        // TODO remove singleton mainwindow
        public static MainWindow Instance { get; private set; }
        
        private MainWindowViewModel? ViewModel => DataContext as MainWindowViewModel;

        private MediaPlayer? Player => ViewModel?.Playback.Player as MediaPlayer;

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
            
            var videoView = this.FindControl<VideoView>("VideoViewLibVlc");
            EngineBindingLibVlc.Bind(Player, videoView);
        }
    }
}