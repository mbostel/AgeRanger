using DebitSuccess.AgeRanger.Api;
using DebitSuccess.AgeRanger.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UI.Desktop {
    public class ContainerBootstrap {

        private IUnityContainer container = new UnityContainer();

        public void Initialise() {

            // File system logger - it's a singleton
            container.RegisterType<ILogger, UI_Logger>(new ContainerControlledLifetimeManager());

            //container.RegisterType<IPersonEntity, PersonEntity>();

            ContainerFactory.Initialise(container);

        }

    }
}
