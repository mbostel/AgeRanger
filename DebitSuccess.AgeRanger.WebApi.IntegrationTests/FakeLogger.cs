using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.WebApi.IntegrationTests {
    public class FakeLogger : ILogger {

        public void Write(string logText, enLogType logType) {
            // No dothing
        }

    }
}
