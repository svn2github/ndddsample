using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDDDSample.Domain.TempHelper
{
    public static class ExtentionMethods
    {
        public static bool IsEmpty<T>(this IList<T> list)
        {
            return list == null || list.Count == 0;
        }
    }
}
