namespace Footage.Service
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Footage.Model;

    public class LocalMediaProvider : MediaProviderBase, ILocalMediaProvider
    {
        private static readonly string[] Extensions = { "mov", "mp4", "mkv" };

        private new LocalMediaSource Source => (LocalMediaSource) base.Source;

        // ReSharper disable once SuggestBaseTypeForParameter
        public LocalMediaProvider(LocalMediaSource source) : base(source)
        {
        }
        
        public override IEnumerable<SourceVideoInfo> FetchVideos()
        {
            var searchOption = Source.IncludeSubfolders ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var files = Directory.EnumerateFiles(Source.RootPath, "*", searchOption).Where(IsFileCompatible); 
            
            return files.Select(filename => new SourceVideoInfo(Source, Path.GetRelativePath(Source.RootPath, filename)));
        }

        protected override string GetFullPathInternal(Video video)
        {
            return Path.Combine(Source.RootPath, video.MediaSourceUri);
        }

        private static bool IsFileCompatible(string path)
        {
            string? extRaw = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extRaw))
                return false;
            
            var ext = extRaw.Substring(1).ToLower();
            return Extensions.Contains(ext);
        }
    }
}