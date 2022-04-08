namespace Footage.UI.EngineBindings
{
    using Footage.Engine;
    using Footage.Engine.MediaPlayer.LibVlc;
    using LibVLCSharp.Avalonia;

    public static class EngineBindingLibVlc
    {
        public static void Bind(IMediaPlayer mediaPlayer, VideoView videoView)
        {
            var libVlcPlayer = mediaPlayer as MediaPlayer;
            videoView.MediaPlayer = libVlcPlayer.Player;
        }
    }
}