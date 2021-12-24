namespace Footage.Model
{
    public class LocalMediaSource : MediaSource
    {
        public string RootPath { get; set; }
        
        public bool IncludeSubfolders { get; set; }
    }
}