namespace Footage.Service.SourceScoped
{
    using System;
    using System.Collections.Generic;
    using Footage.Model;

    public abstract class MediaProviderServiceBase : SourceScopedServiceBase, IMediaProviderService
    {
        public abstract IEnumerable<SourceVideoInfo> FetchVideos();
        
        public abstract string GetFullPath(Video video);

        public abstract DateTime GetDateCreated(Video video);

        protected MediaProviderServiceBase(MediaSource source) : base(source)
        {
        }

        protected void CheckVideo(Video video)
        {
            if (video.MediaSource.Id != Source.Id)
            {
                throw new ArgumentException($"Video {video.Id} does not belong to source {Source.Id} '{Source.Name}'");
            }
        }
    }
}