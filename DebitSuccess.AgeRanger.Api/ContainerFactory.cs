using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    public class ContainerFactory {

        private static IUnityContainer _Container;

        public static IUnityContainer Container { get { return _Container; } }

        public static void Initialise(IUnityContainer container) {
            _Container = container;
        }

        public static T ResolveInstance<T>() {
            try {
                return _Container.Resolve<T>();
            } catch (Exception ex) {
                throw new Exception($"Unable to resolve instance of type {typeof(T)} : {ex.Message}");
            }
        }

        public static T ResolveInstance<T>(string name) {
            try {
                return _Container.Resolve<T>(name);
            } catch (Exception ex) {
                throw new Exception($"Unable to resolve instance of type {typeof(T)} for {name} : {ex.Message}");
            }
        }

     
    }
}
