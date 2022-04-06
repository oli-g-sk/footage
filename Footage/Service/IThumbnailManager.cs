namespace Footage.Service
{
    using System.Drawing;
    using System.Threading.Tasks;
    using Footage.Model;

    /// <summary>
    /// Manages locally stored thumbnails.
    /// </summary>
    public interface IThumbnailManager
    {
        Task<Image?> GetThumbnail(Video video);
    }
}