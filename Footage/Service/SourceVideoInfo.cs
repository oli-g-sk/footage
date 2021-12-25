namespace Footage.Service
{
    using Footage.Model;

    public class SourceVideoInfo
    {
        public MediaSource Source { get; }
        
        public string Identifier { get; }

        public SourceVideoInfo(MediaSource source, string identifier)
        {
            Source = source;
            Identifier = identifier;
        }
    }
}