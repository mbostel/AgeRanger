using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Logging {

    // Use this for Unit Testing
    public class NullLogger : ILogger {

        public void Write(string logText, enLogType logType) {
            // Do nothing
        }

    }
}
