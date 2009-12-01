namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// A value object, as described in the DDD book.
    /// </summary>
    public interface IValueObject<T>
    {
        /// <summary>
        /// Value objects compare by the values of their attributes, 
        /// they don't have an identity.
        /// </summary>      
        bool SameValueAs(T other);
    }
}