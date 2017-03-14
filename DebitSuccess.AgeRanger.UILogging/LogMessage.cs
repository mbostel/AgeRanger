using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.UILogging {
    internal class LogMessage {

        internal DateTime TimeStamp { get; private set; }
        internal enLogType LogType { get; private set; }
        internal string LogText { get; private set; }
        internal int ThreadID { get; private set; }
        internal Guid SessionID { get; private set; }
        internal string LogFileName { get; private set; }

        internal LogMessage(string logText, enLogType logType, string logFileName, Guid sessionID) {

            ThreadID = Thread.CurrentThread.ManagedThreadId;
            TimeStamp = DateTime.Now;

            LogText = logText;
            LogType = logType;
            LogFileName = logFileName;
            SessionID = sessionID;

        }

    }
}
