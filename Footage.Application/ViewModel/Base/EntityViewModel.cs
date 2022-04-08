namespace Footage.Application.ViewModel.Base
{
    using Footage.Model;

    public class EntityViewModel<T> : ViewModelBase, IEntityViewModel where T : IEntity
    {
        public int Id => Item.Id;
        
        public T Item { get; }
        
        public EntityViewModel(T item)
        {
            Item = item;
        }
    }
}