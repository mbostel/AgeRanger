using DebitSuccess.AgeRanger.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Logging {

    /// <summary>
    /// Basic Asynch log writer (the queue decouples log processing from the client). 
    /// Writes to a supplied file. 
    /// Uses SessionIds to identify related log segments.
    /// The log message and associated attributes are instance-based, while the queueing
    /// and processing is all static (ie., single instance) 
    /// </summary>
    public class FileLogger : ILogger {

        private const int LOG_PROCESS_DELAY = 1000;

        // Buffer so we can manage logging asynchronously without impacting upon the performance of the main application functions
        private static Queue<LogMessage> _messageQueue = new Queue<LogMessage>();

        // Make the queue thread safe
        private static object _queueSync = new object();
        private static object _sendSynch = new object();

        // Timer that processes the queue
        private static Timer _queueTimer;

        // Used in the destructor to stop the timer from being recalled
        private static bool _isStopped = false;
        public enLogType LogLevel { get; set; }

        /// <summary>
        /// In this implementation this is the log file name
        /// </summary>
        private string _LogConnectionString;
        public string LogConnectionString {
            get { return _LogConnectionString; }
            set {
                if (value != _LogConnectionString) {
                    _LogConnectionString = value;
                    ValidateLogFile(_LogConnectionString);
                }
            }
        }

        public Guid SessionID { get; set; }

        static FileLogger() {

            // Start the timer
            _queueTimer = new Timer(ProcessQueue, null, LOG_PROCESS_DELAY, Timeout.Infinite);

        }

        /// <summary>
        /// Make sure the queue is empty
        /// </summary>
        ~FileLogger() {
            _isStopped = true;
            Debug.WriteLine("Log Destructor called");
            ProcessQueue(null);
        }

        private void ValidateLogFile(string logFileName) {

            if (string.IsNullOrEmpty(logFileName)) return;

            if (!File.Exists(logFileName)) {
                if (!Directory.Exists(Path.GetDirectoryName(logFileName))) {
                    try {
                        Directory.CreateDirectory(Path.GetDirectoryName(logFileName));
                    } catch (Exception ex) {
                        LogConnectionString = string.Empty;
                        Write(string.Format("Unable to create log file: {0}. Error: {1}", logFileName, ex.GetBaseException().Message), enLogType.Error);
                    }
                }
            }

        }

        public void Write(string logText, enLogType logType) {

            // Only add to the queue if we're supporting this logging type
            if (LogLevel >= logType && LogLevel != enLogType.None) {
                lock (_queueSync) {
                    _messageQueue.Enqueue(new LogMessage(logText, logType, LogConnectionString, SessionID));
                }
            }

        }


        /// <summary>
        /// Empty the message queue into a new bucket and process the bucket in a new thread.
        /// The calling thread is only blocked for as long as it takes to fill the new bucket.
        /// </summary>
        /// <param name="state">Not used</param>
        private static void ProcessQueue(object state) {

            List<LogMessage> messages;

            try {

                lock (_queueSync) {
                    messages = new List<LogMessage>();
                    while (_messageQueue.Count > 0) {
                        messages.Add(_messageQueue.Dequeue());
                    }
                }

                if (messages.Count > 0) {
                    // We don't care what happens to this task.
                    Task.Run(() => { SendLogMessage(messages); });
                }

            } catch (Exception ex) {
                // Discard errors
                Debug.WriteLine(string.Format("Log queue error: {0}", ex.Message));
            } finally {
                if (!_isStopped) {
                    _queueTimer.Change(LOG_PROCESS_DELAY, Timeout.Infinite);
                }
            }

        }

        private static void SendLogMessage(IList<LogMessage> logMessages) {

            try {

                // The lock is more to ensure log order is maintained. There are *probably* no thread issues here
                lock (_sendSynch) {

                    foreach (var msg in logMessages) {
                        
                        // Notify subscribers - if any
                        // MediatorInstance.m_Mediator.NotifyColleagues("LogEvent", msg);

                        string fileText = string.Empty;

                        if (msg.SessionID == Guid.Empty) {
                            fileText = string.Format("{0}{1}: TID: {2} {3}",
                                msg.TimeStamp.ToString("dd-MMM-yyyy HH:mm:ss.fff").PadRight(26, ' '),
                                msg.LogType.ToString().PadRight(8, ' '),
                                msg.ThreadID.ToString("0000"),
                                msg.LogText);
                        } else {
                            fileText = string.Format("{0}{1}: TID: {2} SID: {3} {4}",
                                msg.TimeStamp.ToString("dd-MMM-yyyy HH:mm:ss.fff").PadRight(26, ' '),
                                msg.LogType.ToString().PadRight(8, ' '),
                                msg.ThreadID.ToString("0000"),
                                msg.SessionID.ToString("D"),
                                msg.LogText);
                        }


                        // All log files are validated in the setter
                        if (!string.IsNullOrEmpty(msg.LogFileName)) {
                            using (var streamWriter = File.AppendText(msg.LogFileName)) {
                                if (streamWriter != null) {
                                    streamWriter.Write(fileText + Environment.NewLine);
                                }
                            }
                        }

                        Debug.WriteLine(fileText);
                    }
                }

            } catch (Exception ex) {
                // Eat.
                Debug.WriteLine(string.Format("Log write error: {0}", ex.Message));
            }

        }


    }
}
