namespace Footage.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Footage.Model;

    public class LocalMediaProvider : MediaProviderBase
    {
        private new LocalMediaSource Source => (LocalMediaSource) base.Source;

        // ReSharper disable once SuggestBaseTypeForParameter
        public LocalMediaProvider(LocalMediaSource source) : base(source)
        {
        }
        
        public override IEnumerable<SourceVideoInfo> FetchVideos()
        {
            var searchOption = Source.IncludeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            return Directory.EnumerateFiles(Source.RootPath, "*.mp4", searchOption).Select(filename =>
                new SourceVideoInfo(Source, Path.GetRelativePath(Source.RootPath, filename)));
        }

        protected override string GetFullPathInternal(Video video)
        {
            return Path.Combine(Source.RootPath, video.MediaSourceUri);
        }
    }
}