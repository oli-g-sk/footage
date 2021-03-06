namespace Footage.Application.ViewModel.Section
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Footage.Application.Repository;
    using Footage.Application.Service;
    using Footage.Application.ViewModel.Base;
    using Footage.Application.ViewModel.Entity;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    public class ProjectsViewModel : ItemsAddViewModel<ProjectViewModel, Project>
    {
        private IDialogService DialogService => Locator.Get<IDialogService>();

        private IPersistenceService Settings => Locator.Get<IPersistenceService>();
        
        private ProjectsRepository Repo => Locator.Get<ProjectsRepository>();
        
        public RelayCommand RenameSelectedProjectCommand { get; }
        
        public RelayCommand ArchiveSelectedProjectCommand { get; }

        public ProjectsViewModel()
        {
            RenameSelectedProjectCommand = new RelayCommand(RenameSelectedProject, AnyItemSelected);
            ArchiveSelectedProjectCommand = new RelayCommand(ArchiveSelectedProject, AnyItemSelected);
        }

        protected override async Task<IEnumerable<Project>> LoadAllItems()
        {
            return await Repo.GetAllProjects(false);
        }

        protected override Task OnItemsLoaded()
        {
            int? previouslySelected = Settings.ApplicationData.SelectedProjectId;
            
            if (previouslySelected.HasValue)
            {
                var selectedItem = Items.FirstOrDefault(p => p.Id == previouslySelected);
                
                if (selectedItem != null)
                {
                    Dispatcher.InvokeAsync(() => SelectedItem = selectedItem);
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

            RenameSelectedProjectCommand.RaiseCanExecuteChanged();
            ArchiveSelectedProjectCommand.RaiseCanExecuteChanged();

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
            
            var result = await DialogService.ShowInput("Rename project", "Enter a name for your project", SelectedItem.Name);

            if (result.Confirmed && !string.IsNullOrEmpty(result.InputValue))
            {
                await Repo.RenameProject(SelectedItem.Id, result.InputValue);
            }
        }

        private void ArchiveSelectedProject()
        {
            if (SelectedItem == null)
            {
                return;
            }

            string projectName = SelectedItem.Name;
            int id = SelectedItem.Id;
            
            Dispatcher.InvokeAsync(async () =>
            {
                bool result = await DialogService.ShowYesNo("Archive project",
                    $"Are you sure you want to archive project '{projectName}'?");

                if (result)
                {
                    Items.Remove(SelectedItem);
                    SelectedItem = null;
                    await Repo.ArchiveProject(id);
                }
            });
        }
    }
}