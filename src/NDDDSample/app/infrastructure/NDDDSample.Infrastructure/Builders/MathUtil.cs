namespace NDDDSample.Infrastructure.Builders
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Miscellaneous math utility methods.
    /// </summary>
    /// <remarks>Please be careful in your use of the methods that take left, 
    /// right and epsilon. Because of the imprecise nature of floating point 
    /// calculations, the comparison of <i>epsilon</i> to the difference between 
    /// <i>left</i> and <i>right</i> is not guaranteed to be precise. For example, 
    /// if left is 90.0001, right is 90.0000 and epsilon is 0.0001, this method
    /// might not necessarily return true. I have seen these values produce a
    /// diffence of 0.00010000000000332, which is actually greater than 0.0001.
    /// You must take this fuzzyness into account in your client classes and 
    /// unit tests. This is precisely why we have these epsilon methods in the
    /// first place!
    /// </remarks>
    public class MathUtil
    {
        /// <summary>
        /// Tests whether two float values are equal, within an acceptable margin
        /// of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if the values are equal, within <i>epsilon</i>; otherwise, false</returns>
        /// <see cref="float.Equals"/>
        /// <see href="http://stackoverflow.com/questions/485175"/>
        /// <see href="http://www.brainbell.com/tutorials/C++/Comparing_Floating-Point_Numbers_With_Bounded_Accuracy.htm"/>
        public static bool FloatEqualTo(float left, float right, float epsilon)
        {
            return Math.Abs(left - right) <= epsilon;
        }

        /// <summary>
        /// Tests whether a float value is greater than another float value, 
        /// within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is greater than <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool FloatGreaterThan(float left, float right, float epsilon)
        {
            // delegate
            return FloatGreaterThan(left, right, epsilon, false);
        }

        /// <summary>
        /// Tests whether a float value is greater than or equal to another float
        /// value, within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is greater than or equal to <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool FloatGreaterThanOrEqualTo(float left, float right, float epsilon)
        {
            // delegate
            return FloatGreaterThan(left, right, epsilon, true);
        }

        // impl
        private static bool FloatGreaterThan(float left, float right, float epsilon, bool orEqualTo)
        {
            if (FloatEqualTo(left, right, epsilon))
            {
                // within epsilon, so considered equal
                return orEqualTo;
            }
            return left > right;
        }

        /// <summary>
        /// Tests whether a float value is less than another float value,
        /// within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is less than <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool FloatLessThan(float left, float right, float epsilon)
        {
            // delegate
            return FloatLessThan(left, right, epsilon, false);
        }

        /// <summary>
        /// Tests whether a float value is less than or equal to another float value,
        /// within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is less than or equal to<i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool FloatLessThanOrEqualTo(float left, float right, float epsilon)
        {
            // delegate
            return FloatLessThan(left, right, epsilon, true);
        }

        // impl
        private static bool FloatLessThan(float left, float right, float epsilon, bool orEqualTo)
        {
            if (FloatEqualTo(left, right, epsilon))
            {
                // within epsilon, so considered equal
                return orEqualTo;
            }
            return left < right;
        }

        /// <summary>
        /// Tests whether two double values are equal, within an acceptable margin
        /// of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if the values are equal, within <i>epsilon</i>; otherwise, false</returns>
        /// <see cref="double.Equals"/>
        public static bool DoubleEqualTo(double left, double right, double epsilon)
        {
            return Math.Abs(left - right) <= epsilon;
        }

        /// <summary>
        /// Tests whether a double value is greater than another double value, 
        /// within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is greater than <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool DoubleGreaterThan(double left, double right, double epsilon)
        {
            // delegate
            return DoubleGreaterThan(left, right, epsilon, false);
        }

        /// <summary>
        /// Tests whether a double value is greater than or equal to another double
        /// value, within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is greater than or equal to <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool DoubleGreaterThanOrEqualTo(double left, double right, double epsilon)
        {
            // delegate
            return DoubleGreaterThan(left, right, epsilon, true);
        }

        // impl
        private static bool DoubleGreaterThan(double left, double right, double epsilon, bool orEqualTo)
        {
            if (DoubleEqualTo(left, right, epsilon))
            {
                // within epsilon, so considered equal
                return orEqualTo;
            }
            return left > right;
        }

        /// <summary>
        /// Tests whether a double value is less than another double value, 
        /// within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is less than <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool DoubleLessThan(double left, double right, double epsilon)
        {
            // delegate
            return DoubleLessThan(left, right, epsilon, false);
        }

        /// <summary>
        /// Tests whether a double value is less than or equal to another double
        /// value, within an acceptable margin of difference.
        /// </summary>
        /// <param name="left">the left side of the comparison</param>
        /// <param name="right">the right side of the comparison</param>
        /// <param name="epsilon">the margin of difference</param>
        /// <returns>true if <i>left</i> is less than or equal to <i>right</i>, within <i>epsilon</i>; otherwise, false</returns>
        public static bool DoubleLessThanOrEqualTo(double left, double right, double epsilon)
        {
            // delegate
            return DoubleLessThan(left, right, epsilon, true);
        }

        // impl
        private static bool DoubleLessThan(double left, double right, double epsilon, bool orEqualTo)
        {
            if (DoubleEqualTo(left, right, epsilon))
            {
                // within epsilon, so considered equal
                return orEqualTo;
            }
            return left < right;
        }
    }
}