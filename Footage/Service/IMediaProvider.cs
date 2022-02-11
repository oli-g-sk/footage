namespace Footage.Service
{
    using System.Collections.Generic;
    using Footage.Model;

    public interface IMediaProvider
    {
        IEnumerable<SourceVideoInfo> FetchVideos();

        /// <summary>
        /// Retrieves the full path to the video file for direct playback / streaming.
        /// </summary>
        string GetFullPath(Video video);
    }
}