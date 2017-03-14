using DebitSuccess.AgeRanger.Api;
using DebitSuccess.AgeRanger.Data.SQLite;
using DebitSuccess.AgeRanger.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UnitTests {
    public class ContainerBootstrap {

        private IUnityContainer container = new UnityContainer();

        internal void Initialise() {

            container.RegisterType<IPeopleDataProvider, NullDataProvider>();
            container.RegisterType<ILogger, NullLogger>();
            ContainerFactory.Initialise(container);

        }
    }
}
