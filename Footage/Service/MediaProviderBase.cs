namespace Footage.Service
{
    using System;
    using System.Collections.Generic;
    using Footage.Model;

    public abstract class MediaProviderBase : IMediaProvider
    {
        protected MediaSource Source { get; }
        
        public abstract IEnumerable<SourceVideoInfo> FetchVideos();

        public static MediaProviderBase GetMediaProvider(Video video)
        {
            if (video.MediaSource == null)
            {
                throw new ArgumentException($"GetMediaProvider called with video with NULL MediaSource: {video}");
            }

            return GetMediaProvider(video.MediaSource);
        }

        public static MediaProviderBase GetMediaProvider(MediaSource mediaSource)
        {
            if (mediaSource is LocalMediaSource localMediaSource)
            {
                return new LocalMediaProvider(localMediaSource);
            }

            throw new NotImplementedException($"Unsupported media source: {mediaSource}");
        }

        protected MediaProviderBase(MediaSource source)
        {
            Source = source;
        }

        public string GetFullPath(Video video)
        {
            if (video.MediaSource != Source)
            {
                throw new ArgumentException($"Video {video.Id} does not belong to source {Source.Id} '{Source.Name}'");
            }

            return GetFullPathInternal(video);
        }

        protected abstract string GetFullPathInternal(Video video);
    }
}