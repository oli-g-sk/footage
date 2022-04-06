namespace Footage.Engine;

public class ThumbnailCreationException : Exception
{
    public ThumbnailCreationException(Exception innerException, string message = "Failed to create thumbnail.")
        : base(message, innerException)
    {
    }
}