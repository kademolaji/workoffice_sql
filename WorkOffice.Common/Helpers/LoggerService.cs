

using System;
using NLog;
using WorkOffice.Common.Interfaces;

namespace WorkOffice.Common.Helpers
{
    public class LoggerService : ILoggerService
    {
        private static NLog.ILogger logger = LogManager.GetCurrentClassLogger();

        public void Error(string message)
        {
            logger.Error(message);
        }

        public void Info(string message)
        {
            logger.Info(message);
        }

        public void Debug(string message)
        {
            logger.Debug(message);
        }


        public void Warn(string message)
        {
            logger.Warn(message);
        }
    }
}
