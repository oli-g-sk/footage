using Footage.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage
{
    public class Core
    {
        public static void Initialize()
        {
            Locator.RegisterDefaultDatabase();
            Locator.RegisterDefaultRepositories();
            Locator.RegisterDefaultServices();
            Locator.RegisterDefaultEngine();
        }
    }
}
