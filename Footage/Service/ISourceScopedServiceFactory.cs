using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Service
{
    using Footage.Service.SourceScoped;

    public interface ISourceScopedServiceFactory
    {
        IMediaProviderService GetMediaProviderService(MediaSource mediaSource);
        
        IMediaInfoService GetMediaInfoService(MediaSource mediaSource);

        IThumbnailProvider GetThumbnailProviderService(MediaSource mediaSource);
    }
}
