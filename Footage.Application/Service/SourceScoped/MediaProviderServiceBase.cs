namespace Footage.Application.Service.SourceScoped
{
    using System;
    using System.Collections.Generic;
    using Footage.Model;

    public abstract class MediaProviderServiceBase : SourceScopedServiceBase, IMediaProviderService
    {
        public abstract IEnumerable<SourceVideoInfo> FetchVideos();

        protected MediaProviderServiceBase(MediaSource source) : base(source)
        {
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