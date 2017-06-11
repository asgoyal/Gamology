using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gamology.UserInterface.Services;
using Microsoft.Practices.Unity;

namespace Gamology.UserInterface.Utils.Helpers
{
    public static class ContainerHelper
    {
        private static IUnityContainer _container;
        static ContainerHelper()
        {
            _container = new UnityContainer();
            _container.RegisterType<ICommandHandler, DefaultCommandHandler>(new ContainerControlledLifetimeManager());
        }

        public static IUnityContainer Container => _container;
    }
}
