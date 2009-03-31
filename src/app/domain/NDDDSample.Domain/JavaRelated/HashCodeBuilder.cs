namespace NDDDSample.Domain.JavaRelated
{
    using System;
    using System.Collections.Generic;

    public class HashCodeBuilder
    {
        public HashCodeBuilder Append<T>(IList<T> movements)
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