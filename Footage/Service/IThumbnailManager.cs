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
        Task<string?> GetThumbnail(Video video);
    }
}