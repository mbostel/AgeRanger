using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    public interface ILogMessage {

        DateTime TimeStamp { get; }
        enLogType LogType { get; }
        string LogText { get; }
        int ThreadID { get; }
        Guid SessionID { get; }
        string LogFileName { get; }

    }
}
