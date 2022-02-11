using Footage.Engine;
using Footage.Engine.LibVlc;
using Footage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Win
{
    internal class Provider : IProvider
    {
        public IMediaPlayerService CreateMediaPlayer()
        {
            return new MediaPlayerService();
        }
    }
}
