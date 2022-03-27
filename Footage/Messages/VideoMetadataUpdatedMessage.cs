﻿namespace Footage.Messages
{
    using Footage.Model;

    public class VideoMetadataUpdatedMessage : EntityMessageBase<Video>
    {
        public bool IsMissing { get; }
        
        public long Duration { get; }

        public VideoMetadataUpdatedMessage(Video updatedVideo) : base(updatedVideo)
        {
            IsMissing = updatedVideo.IsMissing;
            Duration = updatedVideo.Duration;
        }
    }
}