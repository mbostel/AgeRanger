using DebitSuccess.AgeRanger.Data.SQLite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UnitTests {

    /// <summary>
    /// Very little we can do here. All the tests belong in Integration
    /// </summary>

    [TestFixture]
    public class SQLite_DataProvider {

        [OneTimeSetUp]
        public void ContainerSetup() {
            new ContainerBootstrap().Initialise();
        }

        [Test]
        public void SQLite_Constructor_NoExceptions() {
            Assert.DoesNotThrow(() => new DataProviderBase());
        }

    }
}
