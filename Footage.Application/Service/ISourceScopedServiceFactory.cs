namespace Footage.Application.Service
{
    using Footage.Application.Service.SourceScoped;
    using Footage.Model;

    public interface ISourceScopedServiceFactory
    {
        IMediaProviderService GetMediaProviderService(MediaSource mediaSource);
        
        IMediaInfoService GetMediaInfoService(MediaSource mediaSource);

        IThumbnailProvider GetThumbnailProviderService(MediaSource mediaSource);
    }
}
