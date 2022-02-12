namespace Footage.ViewModel.Base
{
    using System;
    using System.Threading.Tasks;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    public abstract class ItemsAddViewModel<TViewModel, TModel, TInput> : ItemsViewModel<TViewModel, TModel>
        where TViewModel : EntityViewModel<TModel>
        where TModel : Entity
    {
        public RelayCommand<TInput> AddItemCommand { get; }

        protected ItemsAddViewModel()
        {
            AddItemCommand = new RelayCommand<TInput>(AddItem, CanAddItem);
        }
        
        protected abstract Task<TModel> CreateAndStoreModel(TInput input);
        
        protected virtual void AddItem(TInput input)
        {
            if (!CanAddItem(input))
            {
                throw new InvalidOperationException();
            }
            
            // TODO await
            var task = CreateAndStoreModel(input);
            task.Wait();
            var model = task.Result;

            var viewModel = CreateViewModel(model); 
            Items.Add(viewModel);
            OnItemAdded(viewModel);
        }

        protected virtual void OnItemAdded(TViewModel viewModel)
        {
            
        }

        protected abstract bool CanAddItem(TInput item);
        
        protected abstract TViewModel CreateViewModel(TModel model);
    }
}