using DebitSuccess.AgeRanger.Api;
using DebitSuccess.AgeRanger.Data.SQLite;
using DebitSuccess.AgeRanger.Logging;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DebitSuccess.AgeRanger.WebApi.App_Start {
    internal class ContainerBootstrap {

        private IUnityContainer container = new UnityContainer();

        internal void Initialise() {

            // File system logger - it's a singleton
            container.RegisterType<ILogger, FileLogger>(new ContainerControlledLifetimeManager());

            // SQLite Data provider
            container.RegisterType<IPeopleDataProvider, PeopleProvider>();

            ContainerFactory.Initialise(container);

        }

    }
}