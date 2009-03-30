namespace NDDDSample.Domain.TempHelper
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion

    public class HashCodeBuilder
    {
        public HashCodeBuilder Append<T>(List<T> movements)
        {
            throw new NotImplementedException();
        }

        public int ToHashCode()
        {
            return 0;
        }

        internal HashCodeBuilder Append(object obj)
        {
            throw new NotImplementedException();
        }
    }
}