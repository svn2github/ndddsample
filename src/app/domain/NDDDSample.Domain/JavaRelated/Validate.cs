namespace NDDDSample.Domain.JavaRelated
{
    using System;
    using System.Collections.Generic;

    public class Validate
    {
        public static void NotNull(object id, string msg)
        {
            if (id == null)
            {
                throw new Exception(msg);
            }
        }

        public static void NotNull(object id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("A object instance can't be null");
            }
        }

        public static void IsTrue(bool isTrue, string msg)
        {
            if (!isTrue)
            {
                throw new Exception(msg);
            }
        }

        public static void NoNullElements(object[] objects)
        {
            foreach (object obj in objects)
            {
                NotNull(obj);
            }
        }

        public static void NoNullElements<T>(IList<T> objects)
        {
            foreach (object obj in objects)
            {
                NotNull(obj);
            }
        }

        public static void NotEmpty<T>(IList<T> movements)
        {
            if (movements == null || movements.Count == 0)
            {
                throw new Exception("The list can't be empty");
            }
        }
    }
}