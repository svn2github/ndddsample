namespace NDDDSample.Infrastructure.Validations
{
    #region Usings

    using System;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Provides a set of assertion checks.
    /// </summary>
    public static class Validate
    {
        /// <summary>
        /// Tests if a object is null.
        /// Throws ArgumentNullException with passed message.
        /// </summary>
        /// <param name="theObj">a object instance</param>
        /// <param name="msg">message to throw if the object is null</param>
        public static void NotNull(object theObj, string msg)
        {
            if (theObj == null)
            {
                if (string.IsNullOrEmpty(msg))
                {
                    msg = "A object instance can't be null";
                }

                throw new ArgumentNullException(msg);
            }
        }

        /// <summary>
        /// Tests if a object is null.
        /// Throws ArgumentNullException if object is null.
        /// </summary>
        /// <param name="theObj">a object instance</param>      
        public static void NotNull(object theObj)
        {
            NotNull(theObj, null);
        }

        /// <summary>
        /// Tests if provided bool parameter is true.
        /// Throws ArgumentNullException with passed message if value is false.
        /// </summary>
        /// <param name="isTrue">bool value</param>
        /// <param name="msg">message to throw if the object is null</param>
        public static void IsTrue(bool isTrue, string msg)
        {
            if (!isTrue)
            {
                throw new ArgumentException(msg);
            }
        }

        /// <summary>
        /// Tests if provided array of objects doesnt contain null values.
        /// Throws ArgumentNullException if it has null value.
        /// </summary>
        /// <param name="objects">array of objects</param>
        public static void NoNullElements(object[] objects)
        {
            foreach (object obj in objects)
            {
                NotNull(obj);
            }
        }

        /// <summary>
        /// Tests if provided list of objects doesnt contain null values.
        /// Throws ArgumentNullException if it has null value.
        /// </summary>
        /// <param name="objects">list of objects</param>
        public static void NoNullElements<T>(IList<T> objects)
        {
            foreach (object obj in objects)
            {
                NotNull(obj);
            }
        }

        /// <summary>
        /// Tests if provided list of objects is not null or empty.
        /// Throws ArgumentNullException if it is null or is empty.
        /// </summary>
        /// <param name="objects">list of objects</param>
        public static void NotEmpty<T>(IList<T> objects)
        {
            if (objects == null || objects.Count == 0)
            {
                throw new ArgumentException("The list can't be empty");
            }
        }
    }
}