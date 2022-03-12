using Footage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.ViewModel.Base
{
    public class NamedEntityViewModel<T> : EntityViewModel<T> where T : INamedEntity
    {
        public string Name => Item.Name;

        public NamedEntityViewModel(T item) : base(item)
        {
        }

        public override string ToString() => Name;
    }
}
