using DebitSuccess.AgeRanger.Api;
using Microsoft.Practices.Unity;

namespace DebitSuccess.AgeRanger.Data.SQLite.IntegrationTests {
    public class ContainerBootstrap {

        private IUnityContainer container = new UnityContainer();

        internal void Initialise() {

            container.RegisterType<ILogger, FakeLogger>();
            container.RegisterType<IPersonEntity, PersonEntity>();
            ContainerFactory.Initialise(container);

        }
    }
}
