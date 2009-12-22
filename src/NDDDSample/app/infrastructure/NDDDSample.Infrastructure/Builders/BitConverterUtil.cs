namespace NDDDSample.Infrastructure.Builders
{
    #region Usings

    using System;

    #endregion

    /// <summary>
    /// Utility methods for bit conversion.
    /// </summary>
    public static class BitConverterUtil
    {
        /// <summary>
        /// Returns a representation of the specified floating-point value in
        /// IEEE 754 floating-point single format bit layout. 
        /// </summary>
        /// <param name="value">the floating point number convert</param>
        /// <returns>A 32-bit signed integer whose bits represent the floating point 
        /// number</returns>
        /// <remarks>
        /// If the argument is positive infinity, the result is 0x7f800000.
        /// If the argument is negative infinity, the result is 0xff800000.
        /// If the argument is NaN, the result is 0xffc00000.
        /// </remarks>
        public static int SingleToInt32Bits(float value)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
        }
    }
}