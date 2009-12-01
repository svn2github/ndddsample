namespace NDDDSample.Infrastructure.Log
{
    /// <summary>
    /// Factory class for logging. 
    /// is done for system debug purposes. All other application logging 
    /// necessary for application statistics is outside the scope of this module.
    /// </summary>
    public static class LogFactory
    {
        /// <summary>
        /// The method returns a logger for Application layer.
        /// </summary>
        /// <returns>ILog</returns>
        public static ILog GetApplicationLayerLogger()
        {
            return Log4NetLoggerProxy.GetLogger("ApplicationLayerLogger");
        }
        
        /// <summary>
        /// The method returns a logger for Interface layer.
        /// </summary>
        /// <returns>ILog</returns>
        public static ILog GetInterfaceLayerLogger()
        {
            return Log4NetLoggerProxy.GetLogger("InterfaceLayerLogger");
        }

        /// <summary>
        /// The method returns a logger for External Service.
        /// </summary>
        /// <returns>ILog</returns>
        public static ILog GetExternalServiceLogger()
        {
            return Log4NetLoggerProxy.GetLogger("ExternalServiceLoggerd");
        }
    }
}