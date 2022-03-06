namespace Footage.ViewModel.Base
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Footage.Messages;
    using Footage.Model;
    using GalaSoft.MvvmLight.Command;

    public abstract class ItemsViewModel<TViewModel, TModel> : SectionViewModel
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
                var oldItem = selectedItem;
                BeforeSelectionChanged();
                Set(ref selectedItem, value);
                RemoveSelectedItemCommand.RaiseCanExecuteChanged();
                AfterSelectionChanged();
                MessengerInstance.Send(new SelectionChangedMessage<TViewModel>(oldItem, selectedItem));
            }
        }

        public RelayCommand RemoveSelectedItemCommand { get; } 
        
        protected ItemsViewModel()
        {
            Items = new ObservableCollection<TViewModel>();

            // TODO use async command
            RemoveSelectedItemCommand = new RelayCommand(() => RemoveSelectedItem(), CanRemoveSelectedItem);
        }

        private async Task RemoveSelectedItem()
        {
            if (SelectedItem == null)
            {
                return;
            }
            
            var entity = SelectedItem.Item;
            
            var confirmed = await IsItemRemoveConfirmed(entity);
            if (!confirmed)
            {
                return;
            }
            
            await DeleteModel(entity);
            MessengerInstance.Send(new EntityDeletedMessage<TModel>(entity));
            Items.Remove(SelectedItem);
        }

        protected virtual Task<bool> IsItemRemoveConfirmed(TModel item)
        {
            return Task.FromResult(true);
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