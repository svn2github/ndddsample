namespace NDDDSample.RegisterApp.Log
{
    /// <summary>
    /// Factory class for logging. 
    /// is done for system debug purposes. All other application logging 
    /// necessary for application statistics is outside the scope of this module.
    /// </summary>
    public static class LogFactory
    {
        /// <summary>
        /// The method returns a logger for RegisterApp.
        /// </summary>
        /// <returns>ILog</returns>
        public static ILog GetRegisterAppLogger()
        {
            return Log4NetLoggerProxy.GetLogger("RegisterAppLogger");
        }
    }
}