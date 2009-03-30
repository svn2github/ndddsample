namespace NDDDSample.Domain.Shared
{   
    /// <summary>
    /// A value object, as described in the DDD book.
    /// </summary>
    public interface IValueObject<T>
    {
        /// <summary>
        /// Value objects compare by the values of their attributes, they don't have an identity.
        /// </summary>
        /// <param name="other">The other value object.</param>
        /// <returns>true if the given value object's and this value object's attributes are the same.</returns>
        bool SameValueAs(T other);
    }
}