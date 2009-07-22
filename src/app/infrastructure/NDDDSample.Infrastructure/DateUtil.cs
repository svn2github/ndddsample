namespace NDDDSample.Infrastructure
{
    #region Usings

    using System;
    using System.Globalization;

    #endregion

    /// <summary>
    ///  A few utils for working with Date in tests.
    /// TODO:atrosin revise if its correct place for the class
    /// </summary>
    public static class DateUtil
    {
        /// <summary>
        /// Transforms string to Date
        /// </summary>
        /// <param name="date">Date string as yyyy-MM-dd</param>
        /// <returns>Date representation</returns>
        public static DateTime ToDate(string date)
        {
            return ToDate(date, "00:00");
        }

        public static DateTime ToDate(string date, string time)
        {
            return DateTime.ParseExact(date + " " + time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
        }
    }
}