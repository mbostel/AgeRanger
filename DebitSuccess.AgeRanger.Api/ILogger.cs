using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    public interface ILogger {

        void Write(string logText, enLogType logType);

    }
}
