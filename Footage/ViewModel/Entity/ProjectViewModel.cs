﻿namespace Footage.ViewModel.Entity
{
    using Footage.Messages;
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class ProjectViewModel : NamedEntityViewModel<Project>
    {
        private bool isArchived;

        public bool IsArchived
        {
            get => isArchived;
            set => Set(ref isArchived, value);
        }

        public ProjectViewModel(Project project) : base(project)
        {
            IsArchived = project.IsArchived;
            
            RegisterToEntityMessage<ProjectUpdatedMessage>(OnProjectUpdated);
        }

        private void OnProjectUpdated(ProjectUpdatedMessage message)
        {
            IsArchived = message.IsArchived;
        }
    }
}