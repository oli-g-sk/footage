namespace Footage.UI.EngineBindings
{
    using Footage.Engine;
    using Footage.Engine.LibVlc;
    using LibVLCSharp.Avalonia;

    public static class EngineBindingLibVlc
    {
        public static void Bind(IMediaPlayerService mediaPlayerService, VideoView videoView)
        {
            var libVlcPlayer = mediaPlayerService as MediaPlayerService;
            videoView.MediaPlayer = libVlcPlayer.Player;
        }
    }
}