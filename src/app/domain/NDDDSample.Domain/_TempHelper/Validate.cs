namespace NDDDSample.Domain.TempHelper
{
    #region Usings

    using System;

    #endregion

    public class Validate
    {
        public static void notNull(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Value can't be null");
            }
        }
    }
}