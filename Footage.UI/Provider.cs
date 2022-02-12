using Footage.Engine;
using Footage.Engine.LibVlc;

namespace Footage.UI
{
    internal class Provider : IProvider
    {
        public IDispatcher Dispatcher { get; } = new AvaloniaDispatcher();

        public IMediaPlayer CreateMediaPlayer()
        {
            return new MediaPlayer();
        }
    }
}
