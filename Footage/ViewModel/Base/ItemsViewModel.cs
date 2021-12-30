namespace Footage.ViewModel.Base
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    public abstract class ItemsViewModel<TViewModel, TModel, TInput> : SectionViewModel
        where TViewModel : EntityViewModel<TModel>
        where TModel : Entity
    {
        private TViewModel? selectedItem;
        public ObservableCollection<TViewModel> Items { get; }

        public TViewModel? SelectedItem
        {
            get => selectedItem;
            set
            {
                BeforeSelectionChanged();
                Set(ref selectedItem, value);
                RemoveSelectedItemCommand.RaiseCanExecuteChanged();
                AfterSelectionChanged();
            }
        }

        public RelayCommand<TInput> AddItemCommand { get; }
        
        public RelayCommand RemoveSelectedItemCommand { get; } 
        
        protected ItemsViewModel()
        {
            Items = new ObservableCollection<TViewModel>();

            AddItemCommand = new RelayCommand<TInput>(AddItem, CanAddItem);
            RemoveSelectedItemCommand = new RelayCommand(RemoveSelectedItem, CanRemoveSelectedItem);
        }

        protected abstract TViewModel CreateViewModel(TModel model);

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
            
            Items.Add(CreateViewModel(model));
        }

        protected abstract bool CanAddItem(TInput item);

        private void RemoveSelectedItem()
        {
            // TODO await
            var task = DeleteModel(SelectedItem.Item);
            task.Wait();
            Items.Remove(SelectedItem);
        }

        protected abstract Task DeleteModel(TModel item);

        protected virtual bool CanRemoveSelectedItem()
        {
            return SelectedItem != null;
        }

        protected virtual void BeforeSelectionChanged()
        {
            
        }

        protected virtual void AfterSelectionChanged()
        {
            
        }
    }
}