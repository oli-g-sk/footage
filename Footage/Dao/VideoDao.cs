namespace Footage.Dao
{
    using System.Linq;
    using Footage.Context;
    using Footage.Model;
    using JetBrains.Annotations;

    public class VideoDao : EntityDao<Video>, IVideoDao
    {
    }
}