namespace Footage.Dao
{
    using System.Collections.Generic;
    using System.Linq;
    using Footage.Context;
    using Footage.Model;
    using JetBrains.Annotations;

    public class MediaSourceDao : EntityDao<MediaSource>, IMediaSourceDao
    {
    }
}