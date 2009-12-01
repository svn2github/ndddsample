namespace NDDDSample.Infrastructure.Utils
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Provide a set of utility classes related to Date Time struct.
    /// </summary>
    public static class DateTimeUtil
    {
        /// <summary>
        /// Compare a dateTime is bigger than whenDateTime. 
        /// </summary>
        /// <param name="dateTime">a date time</param>
        /// <param name="whenDateTime">when date time</param>
        /// <returns>true if dateTime is bigger then whenDateTime</returns>
        public static bool After(this DateTime dateTime, DateTime whenDateTime)
        {
            return dateTime > whenDateTime;
        }
        
        /// <summary>
        /// Compare a dateTime is smaller than whenDateTime. 
        /// </summary>
        /// <param name="dateTime">a date time</param>
        /// <param name="whenDateTime">when date time</param>
        /// <returns>true if dateTime is bigger then whenDateTime</returns>
        public static bool Before(this DateTime dateTime, DateTime whenDateTime)
        {
            return dateTime < whenDateTime;
        }
    }
}