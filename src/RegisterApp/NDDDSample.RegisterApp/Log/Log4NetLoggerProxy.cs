namespace NDDDSample.RegisterApp.Log
{
    #region Usings

    using System;
    using log4net;

    #endregion

    internal sealed class Log4NetLoggerProxy : ILog
    {
        private readonly log4net.ILog netLog;

        private Log4NetLoggerProxy(log4net.ILog log)
        {
            netLog = log;
        }

        #region ILog Members

        public void Debug(object message)
        {
            netLog.Debug(message);
        }

        public void Info(object message)
        {
            netLog.Info(message);
        }

        public void Warn(object message)
        {
            netLog.Warn(message);
        }

        public void Error(object message)
        {
            netLog.Error(message);
        }

        public void Fatal(object message)
        {
            netLog.Fatal(message);
        }

        public void Debug(object message, Exception t)
        {
            netLog.Debug(message, t);
        }

        public void Info(object message, Exception t)
        {
            netLog.Info(message, t);
        }

        public void Warn(object message, Exception t)
        {
            netLog.Warn(message, t);
        }

        public void Error(object message, Exception t)
        {
            netLog.Error(message, t);
        }

        public void Fatal(object message, Exception t)
        {
            netLog.Fatal(message, t);
        }

        public void DebugFormat(string format, params object[] args)
        {
            netLog.DebugFormat(format, args);
        }

        public void InfoFormat(string format, params object[] args)
        {
            netLog.InfoFormat(format, args);
        }

        public void WarnFormat(string format, params object[] args)
        {
            netLog.FatalFormat(format, args);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            netLog.ErrorFormat(format, args);
        }

        public void FatalFormat(string format, params object[] args)
        {
            netLog.FatalFormat(format, args);
        }

        #endregion

        public static ILog GetLogger(string loggerType)
        {
            return new Log4NetLoggerProxy(LogManager.GetLogger(loggerType));
        }
    }
}