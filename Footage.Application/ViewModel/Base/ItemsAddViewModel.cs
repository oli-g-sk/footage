namespace Footage.Application.ViewModel.Base
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    public abstract class ItemsAddViewModel<TViewModel, TModel> : ItemsViewModel<TViewModel, TModel>
        where TViewModel : EntityViewModel<TModel>
        where TModel : Entity
    {
        public RelayCommand AddItemCommand { get; }

        protected ItemsAddViewModel()
        {
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);

            Task.Run(async () => 
            { 
                var items = await LoadAllItems();
                
                foreach (var item in items)
                {
                    await Dispatcher.InvokeAsync(() => Items.Add(CreateViewModel(item)));
                }

                await OnItemsLoaded();
            });
        }

        protected abstract Task<IEnumerable<TModel>> LoadAllItems();

        protected virtual Task OnItemsLoaded()
        {
            return Task.CompletedTask;
        }
        
        protected abstract Task<TModel?> CreateAndStoreModel();
        
        private async void AddItem()
        {
            var model = await CreateAndStoreModel();

            if (model == null)
            {
                return;
            }

            var viewModel = CreateViewModel(model); 
            Items.Add(viewModel);
            await OnItemAdded(viewModel);
        }

        protected virtual Task OnItemAdded(TViewModel viewModel)
        {
            return Task.CompletedTask;
        }

        protected virtual bool CanAddItem()
        {
            return true;
        }
        
        protected abstract TViewModel CreateViewModel(TModel model);
    }
}