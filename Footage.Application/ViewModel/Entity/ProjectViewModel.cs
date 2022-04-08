namespace Footage.Application.ViewModel.Entity
{
    using Footage.Application.Messages;
    using Footage.Application.ViewModel.Base;
    using Footage.Model;

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
            
            MessengerHelper.Register<ProjectUpdatedMessage>(this, OnProjectUpdated);
        }

        private void OnProjectUpdated(ProjectUpdatedMessage message)
        {
            IsArchived = message.IsArchived;
        }
    }
}