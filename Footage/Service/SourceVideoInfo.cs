namespace Footage.Service
{
    using Footage.Model;

    public class SourceVideoInfo
    {
        public MediaSource Source { get; init; }
        
        public string Identifier { get; init; }

        public SourceVideoInfo(MediaSource source, string identifier)
        {
            Source = source;
            Identifier = identifier;
        }
    }
}