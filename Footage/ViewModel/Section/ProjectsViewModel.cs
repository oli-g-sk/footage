namespace Footage.ViewModel.Section
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Model;
    using Footage.Repository;
    using Footage.Service;
    using Footage.ViewModel.Base;
    using Footage.ViewModel.Entity;
    using GalaSoft.MvvmLight.Command;

    public class ProjectsViewModel : ItemsAddViewModel<ProjectViewModel, Project>
    {
        private IDialogService DialogService => Locator.Get<IDialogService>();

        private IPersistenceService Settings => Locator.Get<IPersistenceService>();
        
        private ProjectsRepository Repo => Locator.Get<ProjectsRepository>();
        
        public RelayCommand RenameSelectedProjectCommand { get; }

        public ProjectsViewModel()
        {
            RenameSelectedProjectCommand = new RelayCommand(RenameSelectedProject, CanRenameSelectedProject);
        }

        protected override async Task<IEnumerable<Project>> LoadAllItems()
        {
            return await Repo.GetAllProjects();
        }

        protected override Task OnItemsLoaded()
        {
            int? previouslySelected = Settings.ApplicationData.SelectedProjectId;
            
            if (previouslySelected.HasValue)
            {
                var selectedItem = Items.FirstOrDefault(p => p.Id == previouslySelected);
                if (selectedItem != null)
                {
                    SelectedItem = selectedItem;
                }
            }
            
            return Task.CompletedTask;
        }

        protected override async Task<Project?> CreateAndStoreModel()
        {
            var result = await DialogService.ShowInput("New project", "Enter a name for your project", "New project");

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

        protected override Task OnItemAdded(ProjectViewModel viewModel)
        {
            SelectedItem = viewModel;
            return Task.CompletedTask;
        }

        protected override Task DeleteModel(Project item)
        {
            throw new System.NotImplementedException();
        }

        protected override void AfterSelectionChanged()
        {
            base.AfterSelectionChanged();

            Dispatcher.InvokeAsync(() =>
            {
                RenameSelectedProjectCommand.RaiseCanExecuteChanged();
            });

            Settings.ApplicationData.SelectedProjectId = SelectedItem?.Id;
            Settings.UpdateApplicationData();
        }

        // TODO use task
        private async void RenameSelectedProject()
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            var result = await DialogService.ShowInput("New project", "Enter a name for your project", SelectedItem.Name);

            if (result.Confirmed && !string.IsNullOrEmpty(result.InputValue))
            {
                await Repo.RenameProject(SelectedItem.Id, result.InputValue);
            }
        }

        private bool CanRenameSelectedProject()
        {
            return SelectedItem != null;
        }
    }
}