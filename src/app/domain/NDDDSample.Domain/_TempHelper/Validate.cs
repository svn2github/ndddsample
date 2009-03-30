namespace NDDDSample.Domain.TempHelper
{
    #region Usings

    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Model.Voyages;

    #endregion

    public class Validate
    {
        public static void notNull(object id, string msg)
        {
            if (id == null)
            {
                throw new Exception(msg);
            }
        }

        public static void notNull(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("A object instance can't be null");
            }
        }

        public static void isTrue(bool isTrue, string msg)
        {
            if (!isTrue)
            {
                throw new Exception(msg);
            }
        }

        public static void noNullElements(object[] objects)
        {
            foreach (object obj in objects)
            {
                notNull(obj);
            }
        }

        public static void noNullElements<T>(IList<T> objects)
        {
            foreach (object obj in objects)
            {
                notNull(obj);
            }
        }

        public static void notEmpty<T>(IList<T> movements)
        {
            if (movements == null || movements.Count == 0)
            {
                throw new Exception("The list can't be empty");
            }
        }
    }
}