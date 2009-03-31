namespace NDDDSample.Domain.JavaRelated
{
    using System;
    using System.Collections.Generic;

    public static class ExtentionMethods
    {
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static bool After(this DateTime dateTime, DateTime when)
        {
            return dateTime > when;
        }
    }
}