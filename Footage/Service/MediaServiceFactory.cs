using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Service
{
    internal class MediaServiceFactory : IMediaServiceFactory
    {
        public IMediaProviderService GetMediaProvider(MediaSource mediaSource)
        {
            if (mediaSource == null)
            {
                throw new ArgumentNullException(nameof(mediaSource));
            }

            if (mediaSource is LocalMediaSource localMediaSource)
            {
                return new LocalMediaProviderService(localMediaSource);
            }

            throw new NotImplementedException($"Unsupported media source: {mediaSource}");
        }
    }
}
