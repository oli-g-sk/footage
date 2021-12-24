namespace Footage.Service
{
    using System;
    using System.Collections.Generic;
    using Footage.Model;

    public abstract class MediaProviderBase : IMediaProvider
    {
        protected MediaSource Source { get; }
        
        public abstract IEnumerable<SourceVideoInfo> FetchVideos();

        protected MediaProviderBase(MediaSource source)
        {
            Source = source;
        }

        public string GetFullPath(Video video)
        {
            if (video.MediaSourceId != Source.Id)
            {
                throw new ArgumentException($"Video {video.Id} does not belong to source {Source.Id} '{Source.Name}'");
            }

            return GetFullPathInternal(video);
        }

        protected abstract string GetFullPathInternal(Video video);
    }
}