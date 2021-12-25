namespace Footage.Dao
{
    using System.Linq;
    using Footage.Context;
    using Footage.Model;

    public class VideoDao : DaoBase<Video>, IVideoDao
    {
    }
}