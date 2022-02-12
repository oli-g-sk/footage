namespace Footage.Service.SourceScoped
{
    using Footage.Model;

    public class SourceScopedServiceBase
    {
        protected MediaSource Source { get; }
        
        protected SourceScopedServiceBase(MediaSource source)
        {
            Source = source;
        }
    }
}