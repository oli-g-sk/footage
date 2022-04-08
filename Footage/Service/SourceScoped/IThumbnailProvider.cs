namespace Footage.Service.SourceScoped
{
    using System.Drawing;
    using System.Threading.Tasks;
    using Footage.Model;

    /// <summary>
    /// Provides a default thumbnail for a given video, depending on its media source.
    /// </summary>
    public interface IThumbnailProvider
    {
        Task<Image?> GetDefaultThumbnail(Video video);
    }
}