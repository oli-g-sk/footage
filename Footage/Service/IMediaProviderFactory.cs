using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Service
{
    public interface IMediaProviderFactory
    {
        MediaProviderBase GetMediaProvider(Video video);

        MediaProviderBase GetMediaProvider(MediaSource mediaSource);
    }
}
