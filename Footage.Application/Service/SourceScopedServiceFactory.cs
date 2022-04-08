namespace Footage.Application.Service
{
    using System;
    using Footage.Application.Service.SourceScoped;
    using Footage.Model;

    internal class SourceScopedServiceFactory : ISourceScopedServiceFactory
    {
        public IMediaProviderService GetMediaProviderService(MediaSource mediaSource)
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

        public IMediaInfoService GetMediaInfoService(MediaSource mediaSource)
        {
            if (mediaSource == null)
            {
                throw new ArgumentNullException(nameof(mediaSource));
            }

            if (mediaSource is LocalMediaSource localMediaSource)
            {
                return new LocalMediaInfoPlayerService();
            }

            throw new NotImplementedException($"Unsupported media source: {mediaSource}");
        }

        public IThumbnailProvider GetThumbnailProviderService(MediaSource mediaSource)
        {
            if (mediaSource == null)
            {
                throw new ArgumentNullException(nameof(mediaSource));
            }

            if (mediaSource is LocalMediaSource localMediaSource)
            {
                var mediaProvider = new LocalMediaProviderService(localMediaSource);
                return new LocalThumbnailProvider(mediaProvider, localMediaSource);
            }

            throw new NotImplementedException($"Unsupported media source: {mediaSource}");
        }
    }
}
