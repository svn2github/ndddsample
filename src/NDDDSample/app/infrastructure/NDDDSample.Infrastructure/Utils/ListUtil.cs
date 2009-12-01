namespace NDDDSample.Infrastructure.Utils
{
    #region Usings

    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Provide a set of utility classes related to Lists.
    /// </summary>
    public static class ListUtil
    {
        /// <summary>
        /// Tests if the list is is null or empty.
        /// </summary>
        /// <typeparam name="T">List Type</typeparam>
        /// <param name="list">a list</param>
        /// <returns>true if the list is null or empty</returns>
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}