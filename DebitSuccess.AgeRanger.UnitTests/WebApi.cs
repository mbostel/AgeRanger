using DebitSuccess.AgeRanger.WebApi.Controllers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UnitTests {

    /// <summary>
    /// Very little we can do here. All the tests belong in Integration.
    /// There are a few methods that could have been unit-tested, but then
    /// they would have had to have been made public
    /// </summary>

    [TestFixture]
    public class WebApi {

        [OneTimeSetUp]
        public void ContainerSetup() {
            new ContainerBootstrap().Initialise();
        }

        /// <summary>
        /// Test the constructor. Just ensure it doesn't throw an exception
        /// </summary>
        [Test]
        public void WebAPI_Constructor_NoExceptions() {
            Assert.DoesNotThrow(() => new PersonController());
        }


        

    }
}
