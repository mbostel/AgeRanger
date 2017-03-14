using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Logging {
    internal class LogMessage : ILogMessage {

        public DateTime TimeStamp { get; private set; }
        public enLogType LogType { get; private set; }
        public string LogText { get; private set; }
        public int ThreadID { get; private set; }
        public Guid SessionID { get; private set; }
        public string LogFileName { get; private set; }

        public LogMessage(string logText, enLogType logType, string logFileName, Guid sessionID) {

            ThreadID = Thread.CurrentThread.ManagedThreadId;
            TimeStamp = DateTime.Now;

            LogText = logText;
            LogType = logType;
            LogFileName = logFileName;
            SessionID = sessionID;

        }

    }
}
