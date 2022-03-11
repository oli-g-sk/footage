namespace Footage.ViewModel.Base
{
    using System;
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
        }
        
        protected abstract Task<TModel?> CreateAndStoreModel();
        
        private void AddItem()
        {
            // TODO await
            var task = CreateAndStoreModel();
            task.Wait();
            var model = task.Result;

            if (model == null)
            {
                return;
            }

            var viewModel = CreateViewModel(model); 
            Items.Add(viewModel);
            OnItemAdded(viewModel);
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