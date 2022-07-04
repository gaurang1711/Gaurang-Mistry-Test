using System;
using System.Configuration;
using System.IO;
using NLog;

namespace PolicyProcessor
{
    public class Logger
    {
        private string location { get; set; }

        /// <summary>
        /// Set log file location
        /// </summary>
        /// <param name="LogFielLocation">Log File Location</param>
        public Logger(string LogFielLocation)
        {
            this.location = LogFielLocation;
        }

        #region [ Get Logger ]
        /// <summary>
        /// Get Log file and detail
        /// </summary>
        /// <returns>Return Log Detail</returns>
        public NLog.Logger getLogger()
        {
            var config = new NLog.Config.LoggingConfiguration();
            var path = location;
            var fileName = ConfigurationManager.AppSettings["LogFileName"] + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            string file = Path.Combine(location, fileName);

            var logFile = new NLog.Targets.FileTarget("logFile") { FileName = file };
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFile);

            
            LogManager.Configuration = config;
            NLog.Logger logger = LogManager.GetCurrentClassLogger();

            return logger;
        }
        #endregion

        #region [ Write Log File ]
        /// <summary>
        /// Set Log Information to file
        /// </summary>
        /// <param name="message">Log Message</param>
        public void Info(string message)
        {
            NLog.Logger logger = getLogger();
            logger.Info(message);
        }
        #endregion

        #region [ Write Log Error ]
        /// <summary>
        /// Set Log Error to file
        /// </summary>
        /// <param name="message">Log error message</param>
        public void Error(string message)
        {
            NLog.Logger logger = getLogger();
            logger.Error(message);
        }
        #endregion
    }
}
