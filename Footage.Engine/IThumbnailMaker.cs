namespace Footage.Engine
{
    using System.Drawing;
    using System.Threading.Tasks;

    public interface IThumbnailMaker
    {
        // TODO add support for specifying time
        Task CreateThumbnail(string mediaPath, string outputPath, int width);
    }
}
