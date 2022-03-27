namespace Footage.Messages
{
    using System;
    using Footage.Model;

    public class ProjectUpdatedMessage : EntityMessageBase
    {
        public bool IsArchived { get; }
        
        public ProjectUpdatedMessage(Project updatedProject) : base(updatedProject)
        {
            if (updatedProject == null) 
                throw new ArgumentNullException(nameof(updatedProject));

            IsArchived = updatedProject.IsArchived;
        }
    }
}