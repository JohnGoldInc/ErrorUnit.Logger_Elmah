using Elmah;
using ErrorUnit.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ErrorUnit.Logger_Elmah
{
    /// <summary>
    /// The Elmah ErrorUnit Logger class
    /// </summary>
    public class ErrorUnitLogger : ILogger
    {
        /// <summary>
        /// Logs the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public void Log(Exception ex)
        {
            var signal = ErrorSignal.FromCurrentContext();
            if (signal == null)
                return;
            signal.Raise(ex);
        }

        /// <summary>
        /// Logs the specified testable error json.
        /// </summary>
        /// <param name="testableErrorJson">The testable error json.</param>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
        public string Log(string testableErrorJson, Exception exception)
        {
            var signal = ErrorSignal.FromCurrentContext();

            var log = new ErrorUnitException(testableErrorJson, exception);
            if (signal != null)
                signal.Raise(log);
            return log.ToString();
        }

        /// <summary>
        /// Gets the error unit json after-date.
        /// </summary>
        /// <param name="afterdate">The after-date.</param>
        /// <returns></returns>
        public IEnumerable<string> GetErrorUnitJson(DateTime afterdate)
        {
            var a = new Elmah.ErrorLogDataSourceAdapter();
            var errLog = Elmah.ErrorLog.GetDefault(null);

            const int rowsize = 10;
            var startrow = 0;
            var testableErrors = new ConcurrentBag<string>();
            ErrorLogEntry[] logentries = null;
            while (logentries == null || logentries.Count() == rowsize)
            {
                logentries = a.GetErrors(startrow, rowsize);
                startrow += rowsize;

                foreach (var te in logentries.Where(le => le.Error.Time > afterdate))
                {
                    var err = errLog.GetError(te.Id);
                    var detail = err.Error.Detail?.Trim();
                    if (detail.EndsWith("}") && detail.Contains(@"""$type"": ""ErrorUnit.Models.TestableError, ErrorUnit"","))
                        testableErrors.Add(detail.Substring(detail.IndexOf('{')));

                }

                if (logentries.Any(le => le.Error.Time <= afterdate))
                    break;
            }


            return testableErrors;
        }

    }
}
