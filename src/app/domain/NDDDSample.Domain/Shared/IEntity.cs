namespace NDDDSample.Domain.Shared
{
    /// <summary>
    /// An entity, as explained in the DDD book.
    /// </summary>  
    public interface IEntity<T>
    {
        /// <summary>
        /// Entities compare by identity, not by attributes.
        /// </summary>
        /// <param name="other">The other entity.</param>
        /// <returns>true if the identities are the same, regardles of other attributes.</returns>
        bool SameIdentityAs(T other);
    }
}