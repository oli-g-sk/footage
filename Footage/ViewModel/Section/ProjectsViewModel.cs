namespace Footage.ViewModel.Section
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;

    public class ProjectsViewModel : ItemsAddViewModel<ProjectViewModel, Project>
    {
        private ProjectsRepository Repo => Locator.Get<ProjectsRepository>();

        protected override async Task<IEnumerable<Project>> LoadAllItems()
        {
            return await Repo.GetAllProjects();
        }

        protected override async Task<Project?> CreateAndStoreModel()
        {
            var dialogService = Locator.Get<IDialogService>();
            var result = await dialogService.ShowInput("New project", "Enter a name for your project", "New project");

            if (result.Confirmed && !string.IsNullOrEmpty(result.InputValue))
            {
                var project = await Repo.CreateNewProject(result.InputValue);
                return project;
            }

            return null;
        }

        protected override ProjectViewModel CreateViewModel(Project model)
        {
            return new(model);
        }
        
        protected override Task DeleteModel(Project item)
        {
            throw new System.NotImplementedException();
        }
    }
}