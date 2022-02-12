using Footage.Engine;
using Footage.Engine.LibVlc;

namespace Footage.UI
{
    internal class Provider : IProvider
    {
        private IDispatcher dispatcher;

        internal Provider()
        {
            dispatcher = new AvaloniaDispatcher();
        }

        public IDispatcher Dispatcher => dispatcher;

        public IMediaPlayerService CreateMediaPlayer()
        {
            return new MediaPlayerService();
        }
    }
}
