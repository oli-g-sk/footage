namespace Footage.ViewModel
{
    using Footage.Model;
    using GalaSoft.MvvmLight;

    public class EntityViewModel<T> : ViewModelBase where T : Entity
    {
        public T Item { get; }
        
        public EntityViewModel(T item)
        {
            Item = item;
        }
    }
}