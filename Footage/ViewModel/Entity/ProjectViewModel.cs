namespace Footage.ViewModel.Entity
{
    using Footage.Model;
    using Footage.ViewModel.Base;

    public class ProjectViewModel : EntityViewModel<Project>
    {
        public string Name => Item.Name;
        
        public ProjectViewModel(Project project) : base(project)
        {
        }
    }
}