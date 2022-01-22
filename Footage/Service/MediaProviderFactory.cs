using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Service
{
    internal class MediaProviderFactory : IMediaProviderFactory
    {
        public MediaProviderBase GetMediaProvider(Video video)
        {
            if (video.MediaSource == null)
            {
                throw new ArgumentException($"GetMediaProvider called with video with NULL MediaSource: {video}");
            }

            return GetMediaProvider(video.MediaSource);
        }

        public MediaProviderBase GetMediaProvider(MediaSource mediaSource)
        {
            if (mediaSource == null)
            {
                throw new ArgumentNullException(nameof(mediaSource));
            }

            if (mediaSource is LocalMediaSource localMediaSource)
            {
                return new LocalMediaProvider(localMediaSource);
            }

            throw new NotImplementedException($"Unsupported media source: {mediaSource}");
        }
    }
}
