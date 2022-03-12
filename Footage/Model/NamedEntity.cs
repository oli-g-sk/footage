using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.Model
{
    public class NamedEntity : Entity, INamedEntity
    {
        public string? Name { get; set; }
    }
}
