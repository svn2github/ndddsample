namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// Utility code for domain classes.
    /// </summary>
    public static class DomainObjectUtils
    {
        /// <summary>
        /// Null safe operation.
        /// </summary>
        /// <typeparam name="T">type</typeparam>
        /// <param name="actual">actual value</param>
        /// <param name="safe">a null-safe value</param>
        /// <returns>actual value, if it's not null, or safe value if the actual value is null.</returns>
        public static T NullSafe<T>(T actual, T safe)
            where T : class
        {
            return actual ?? safe;
        }

        // TODO wrappers for some of the commons-lang code:
        // EqualsBuilder that uses sameIdentity/sameValue,
        // better validation (varargs etc)         
    }
}