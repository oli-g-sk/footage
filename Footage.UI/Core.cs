using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage.UI
{
    public static class Core
    {
        public static void Initialize()
        {
            Footage.Core.Initialize(new Provider());
        }
    }
}
