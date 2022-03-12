namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class ProjectViewModel : NamedEntityViewModel<Project>
    {        
        public ProjectViewModel(Project project) : base(project)
        {
        }
    }
}