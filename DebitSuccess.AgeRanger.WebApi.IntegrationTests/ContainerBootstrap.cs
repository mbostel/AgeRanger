using DebitSuccess.AgeRanger.Api;
using Microsoft.Practices.Unity;

namespace DebitSuccess.AgeRanger.WebApi.IntegrationTests {
    public class ContainerBootstrap {

        private IUnityContainer container = new UnityContainer();

        internal void Initialise() {

            container.RegisterType<IPersonEntity, PersonEntity>();
            container.RegisterType<ILogger, FakeLogger>();
            ContainerFactory.Initialise(container);

        }
    }
}
