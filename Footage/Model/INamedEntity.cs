using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Model
{
    // TODO make this interface (and parent IEntity) INTERNAL ?
    public interface INamedEntity : IEntity
    {
        string Name { get; }
    }
}
