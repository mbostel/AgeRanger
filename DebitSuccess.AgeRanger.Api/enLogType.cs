using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebitSuccess.AgeRanger.Api {
    /// <summary>
    /// This arrangement supports setting a logging level in addition
    /// to enumerating the log types:
    /// 
    ///     Level 0: All Logging OFF
    ///     Level 1: Only log errors
    ///     Level 2: Only Error & Warnings
    ///     Level 3: Information Messages
    ///     Level 4: Trace messages (All Logging ON)
    /// 
    /// </summary>
    public enum enLogType {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Trace = 4
    }
}
