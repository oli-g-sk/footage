using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.ViewModel.Base
{
    using Footage.Messages;

    public class NamedEntityViewModel<T> : EntityViewModel<T> where T : INamedEntity
    {
        private string? name;

        public string? Name
        {
            get => name;
            protected set => Set(ref name, value);
        }

        public NamedEntityViewModel(T item) : base(item)
        {
            Name = item.Name;
            MessengerHelper.Register<EntityRenamedMessage<T>>(this, OnEntityRenamed);
        }
        
        private void OnEntityRenamed(EntityRenamedMessage<T> message)
        {
            Name = message.Name;
        }
        
        public override string ToString() => Name;
    }
}
