using Footage.Engine;
using Footage.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage
{
    /// <summary>
    /// Implement this interface to plug in dependencies.
    /// </summary>
    public interface IProvider
    {
        IMediaPlayerService CreateMediaPlayer();
    }
}
